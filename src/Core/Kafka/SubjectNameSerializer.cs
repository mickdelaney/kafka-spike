using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avro.IO;
using Avro.Specific;
using Confluent.SchemaRegistry;

namespace Messages
{
    
    /// <summary>
    /// Like the Confluent SpecificSerializerImpl but using the TopicSubjectSchemaCache
    /// to provide Subjects 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class SubjectNameSerializer<T>
    {
        readonly ISchemaRegistryClient _schemaRegistryClient;

        readonly bool _autoRegisterSchema;
        readonly int _initialBufferSize;

        int? _writerSchemaId;
        string _writerSchemaString;
        global::Avro.Schema _writerSchema;

        SpecificWriter<T> _avroWriter;
        
        readonly HashSet<string> _subjectsRegistered = new HashSet<string>();
        readonly SemaphoreSlim _serializeMutex = new SemaphoreSlim(1);

        public SubjectNameSerializer
        (
            ISchemaRegistryClient schemaRegistryClient,
            bool autoRegisterSchema,
            int initialBufferSize
        )
        {
            _schemaRegistryClient = schemaRegistryClient;
            _autoRegisterSchema = autoRegisterSchema;
            _initialBufferSize = initialBufferSize;
        }

        
        public async Task<byte[]> Serialize(string topic, T data, bool isKey)
        {
            try
            {
                // We need the topic name when creating the 
                if (_writerSchema == null)
                {
                    _writerSchema = (global::Avro.Schema)typeof(T).GetField("_SCHEMA", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                    _writerSchemaString = _writerSchema.ToString();    
                    _avroWriter = new SpecificWriter<T>(_writerSchema);
                }
                
                
                await _serializeMutex.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
                
                try
                {
                    var subject = isKey ? SubjectNameFactory.KeySubjectNameFrom<T>(topic) : SubjectNameFactory.ValueSubjectNameFrom<T>(topic);

                    if (!_subjectsRegistered.Contains(subject))
                    {
                        // first usage: register/get schema to check compatibility
                        _writerSchemaId = _autoRegisterSchema
                            ? await _schemaRegistryClient.RegisterSchemaAsync(subject, _writerSchemaString).ConfigureAwait(continueOnCapturedContext: false)
                            : await _schemaRegistryClient.GetSchemaIdAsync(subject, _writerSchemaString).ConfigureAwait(continueOnCapturedContext: false);

                        _subjectsRegistered.Add(subject);
                    }
                }
                finally
                {
                    _serializeMutex.Release();
                }

                if (_writerSchemaId.HasValue == false)
                {
                    throw new Exception("Not SchemaId Available For Message");
                }
                
                using (var stream = new MemoryStream(_initialBufferSize))
                using (var writer = new BinaryWriter(stream))
                {
                    stream.WriteByte(ConfluentConstants.MagicByte);

                    writer.Write(IPAddress.HostToNetworkOrder(_writerSchemaId.Value));
                    _avroWriter.Write(data, new BinaryEncoder(stream));

                    // TODO: maybe change the ISerializer interface so that this copy isn't necessary.
                    return stream.ToArray();
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