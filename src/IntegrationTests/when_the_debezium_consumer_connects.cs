using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.SchemaRegistry;
using Core;
using Messages;
using Shouldly;
using Xunit;

namespace IntegrationTests
{
    public class when_the_debezium_consumer_connects
    {
        readonly KafkaConfig _kafkaConfig = new KafkaConfig();

        const string TopicName = "";
        
        [Fact]
        public async Task It_should_deserialize_the_envelope_correctly()
        {
            var schemaRegistry = new CachedSchemaRegistryClient
            (
                new SchemaRegistryConfig { SchemaRegistryUrl = _kafkaConfig.SchemaRegistryUrl }
            );

            
            //var type = AvroMultipleDeserializer.Get(schema.Name);
            
            var subjects = await schemaRegistry.GetAllSubjectsAsync();
            
            subjects.Count.ShouldBePositive();

            foreach (var subject in subjects)
            {
                var schemas = await schemaRegistry.GetLatestSchemaAsync(subject);

                var schemaSubject = await schemaRegistry.GetSchemaAsync(1);
                var schema = global::Avro.Schema.Parse(schemaSubject);
            }
        }
    }
}