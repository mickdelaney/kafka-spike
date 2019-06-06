using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Elevate.Accounts;
using Messages;

namespace Core
{
    public class UserProducer
    {
        readonly KafkaConfig _config;
        readonly CancellationTokenSource _cts;
        readonly string _name;
        readonly string _topicName;
        readonly TopicSubjectSchemaCache _cache;

        public UserProducer
        (
            KafkaConfig config,
            CancellationTokenSource cts,
            string name,
            string topicName,
            TopicSubjectSchemaCache cache
        )
        {
            _config = config;
            _cts = cts;
            _name = name;
            _topicName = topicName;
            _cache = cache;
        }
        
        public async Task Produce()
        {
            using (var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { SchemaRegistryUrl = _config.SchemaRegistryUrl }))
            using (var producer = Build(schemaRegistry))
            {
                var testUsers = new Faker<User>()
                    .RuleFor(u => u.id, f => Guid.NewGuid().ToString())
                    .RuleFor(u => u.first_name, (f, u) => f.Name.FirstName())
                    .RuleFor(u => u.last_name, (f, u) => f.Name.LastName())
                    .RuleFor(u => u.email_name, (f, u) => f.Internet.Email(u.first_name, u.last_name));
                    
                var i = 0;

                while (_cts.IsCancellationRequested == false)
                {
                    var user = testUsers.Generate();
                    
                    await producer
                        .ProduceAsync(_topicName, new Message<string, User> { Key = i++.ToString(), Value = user})
                        .ContinueWith(LogError);
                  
                    producer.Flush(TimeSpan.FromSeconds(5));
                }
            }
        }

        IProducer<string, User> Build(CachedSchemaRegistryClient schemaRegistry)
        {
            return new ProducerBuilder<string, User>(new ProducerConfig { BootstrapServers = _config.Brokers })
                .SetKeySerializer(new AvroSerializer<string>(schemaRegistry))
                .SetValueSerializer(new AvroTopicSubjectSchemaCacheSerializer<User>(schemaRegistry, _cache))
                .Build();
        }
        
        async Task LogError(Task<DeliveryResult<string, User>> task)
        {
            if (task.Exception == null)
            {
                return;
            }

            var message = task.IsFaulted
                ? $"Producer: {_name}, error producing message: {task.Exception.Message}"
                : $"produced to: {task.Result.TopicPartitionOffset}";
            
            await Console.Out.WriteLineAsync(message);
        }
    }
}