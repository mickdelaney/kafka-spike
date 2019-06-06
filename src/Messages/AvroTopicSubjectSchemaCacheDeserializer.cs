using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Avro.Generic;
using Avro.IO;
using Avro.Specific;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Schema = Avro.Schema;

namespace Messages
{
    public class AvroTopicSubjectSchemaCacheDeserializer : IAsyncDeserializer<object>
    {
        /// <remarks>
        ///     A datum reader cache (one corresponding to each write schema that's been seen) 
        ///     is maintained so that they only need to be constructed once.
        /// </remarks>
        readonly Dictionary<int, DatumReader<object>> _datumReaderBySchemaId = new Dictionary<int, DatumReader<object>>();

        readonly SemaphoreSlim _deserializeMutex = new SemaphoreSlim(1);

        readonly ISchemaRegistryClient _schemaRegistryClient;
        readonly TopicSubjectSchemaCache _cache;

        public AvroTopicSubjectSchemaCacheDeserializer
        (
            ISchemaRegistryClient schemaRegistryClient, 
            TopicSubjectSchemaCache cache
        )
        {
            _schemaRegistryClient = schemaRegistryClient;
            _cache = cache;
        }

        Schema GetReaderSchema(Schema subject)
        {
            return _cache.GetSchema(subject);
        }
     
        public async Task<object> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context)
        {
            try
            {
                // Note: topic is not necessary for deserialization (or knowing if it's a key 
                // or value) only the schema id is needed.

                using (var stream = new MemoryStream(data.ToArray()))
                using (var reader = new BinaryReader(stream))
                {
                    var magicByte = reader.ReadByte();
                    if (magicByte != ConfluentConstants.MagicByte)
                    {
                        // may change in the future.
                        throw new InvalidDataException($"magic byte should be {ConfluentConstants.MagicByte}, not {magicByte}");
                    }
                    
                    var writerId = IPAddress.NetworkToHostOrder(reader.ReadInt32());

                    DatumReader<object> datumReader;
                    
                    await _deserializeMutex.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
                    
                    try
                    {
                        _datumReaderBySchemaId.TryGetValue(writerId, out datumReader);
                        
                        if (datumReader == null)
                        {
                            if (_datumReaderBySchemaId.Count > _schemaRegistryClient.MaxCachedSchemas)
                            {
                                _datumReaderBySchemaId.Clear();
                            }

                            var writerSchemaJson = await _schemaRegistryClient.GetSchemaAsync(writerId).ConfigureAwait(continueOnCapturedContext: false);
                            var writerSchema = global::Avro.Schema.Parse(writerSchemaJson);

                            var readerSchema = GetReaderSchema(writerSchema);
                            
                            datumReader = new SpecificReader<object>(writerSchema, readerSchema);
                            
                            _datumReaderBySchemaId[writerId] = datumReader;
                        }
                    }
                    finally
                    {
                        _deserializeMutex.Release();
                    }
                    
                    return datumReader.Read(default, new BinaryDecoder(stream));
                }
            }
            catch (AggregateException e)
            {
                if (e.InnerException == null)
                {
                    throw;
                }

                throw e.InnerException;
            }
        }

    }
}