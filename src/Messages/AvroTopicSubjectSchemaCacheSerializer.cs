using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avro.Generic;
using Bogus;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace Messages
{
    public class AvroTopicSubjectSchemaCacheSerializer<T> : IAsyncSerializer<T>
    {
        public const int DefaultInitialBufferSize = 1024;

        readonly ISchemaRegistryClient _schemaRegistryClient;
        readonly TopicSubjectSchemaCache _cache;

        SpecificSerializer<T> _serializer;

        public AvroTopicSubjectSchemaCacheSerializer
        (
            ISchemaRegistryClient schemaRegistryClient, 
            TopicSubjectSchemaCache cache
        )
        {
            _schemaRegistryClient = schemaRegistryClient;
            _cache = cache;
        }

        public async Task<byte[]> SerializeAsync(T value, SerializationContext context)
        { 
            try
            {
                if (_serializer == null)
                {
                    _serializer = new SpecificSerializer<T>
                    (
                        _schemaRegistryClient, 
                        true, 
                        DefaultInitialBufferSize,
                        _cache
                    );
                }

                return await _serializer.Serialize(context.Topic, value, context.Component == MessageComponentType.Key);
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