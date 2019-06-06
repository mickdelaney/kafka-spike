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
    public class AvroSubjectNameStrategySerializer<T> : IAsyncSerializer<T>
    {
        public const int DefaultInitialBufferSize = 1024;

        readonly ISchemaRegistryClient _schemaRegistryClient;

        SubjectNameSerializer<T> _serializer;

        public AvroSubjectNameStrategySerializer
        (
            ISchemaRegistryClient schemaRegistryClient
        )
        {
            _schemaRegistryClient = schemaRegistryClient;
        }

        public async Task<byte[]> SerializeAsync(T value, SerializationContext context)
        { 
            try
            {
                if (_serializer == null)
                {
                    _serializer = new SubjectNameSerializer<T>
                    (
                        _schemaRegistryClient, 
                        true, 
                        DefaultInitialBufferSize
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