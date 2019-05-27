using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Elevate.Accounts;

namespace Core
{
    public class UserConsumer
    {
        readonly KafkaConfig _config;
        readonly CancellationTokenSource _cts;
        readonly string _name;
        readonly string _topicName;

        public UserConsumer
        (
            KafkaConfig config,
            CancellationTokenSource cts,
            string name,
            string topicName
        )
        {
            _config = config;
            _cts = cts;
            _name = name;
            _topicName = topicName;
        }
        
        public async Task Consume()
        {
            using (var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { SchemaRegistryUrl = _config.SchemaRegistryUrl }))
            {
                using (var consumer = Build(schemaRegistry))
                {
                    consumer.Subscribe(_topicName);

                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var result = consumer.Consume(_cts.Token);

                                await Console.Out.WriteLineAsync($"User key name: {result.Message.Key}, user value first_name: {result.Value.first_name}");

                                var offsets = consumer.Commit();
                            
                            }
                            catch (ConsumeException e)
                            {
                                await Console.Out.WriteLineAsync($"Consume error: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumer.Close();
                    }
                }
            }
        }

        IConsumer<string, User> Build(CachedSchemaRegistryClient schemaRegistry)
        {
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config.Brokers,
                GroupId = _config.UserConsumerGroupId,
                EnableAutoCommit = false,
                StatisticsIntervalMs = 5000,
                SessionTimeoutMs = 6000,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };
            
            return new ConsumerBuilder<string, User>(consumerConfig)
                .SetKeyDeserializer(new AvroDeserializer<string>(schemaRegistry).AsSyncOverAsync())
                .SetValueDeserializer(new AvroDeserializer<User>(schemaRegistry).AsSyncOverAsync())
                .SetErrorHandler(LogError)
                .Build();
        }
        
        async void LogError(IConsumer<string, User> consumer, Error error)
        {
            if (error == null)
            {
                return;
            }

            var message = $"Consumer: {_name}. Error: Code {error.Code}, Reason: {error.Reason}";
            
            await Console.Out.WriteLineAsync(message);
        }
    }
}