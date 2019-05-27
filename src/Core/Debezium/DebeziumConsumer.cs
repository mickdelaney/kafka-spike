using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Core.Debezium;
using Core.Domain.Rates;
using Elevate.Accounts;
using Newtonsoft.Json;

namespace Core
{
    public class DebeziumConsumer
    {
        readonly KafkaConfig _config;
        readonly CancellationTokenSource _cts;
        readonly string _name;
        readonly string _topicName;

        public DebeziumConsumer
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

                                var envelope = JsonConvert.DeserializeObject<DebeziumEnvelope<ApplicationRate>>(result.Value);
                                
                                await Console.Out.WriteLineAsync($"User key name: {result.Message.Key}, user value first_name: {result.Value}");

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

        IConsumer<Ignore, string> Build(CachedSchemaRegistryClient schemaRegistry)
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
            
            return new ConsumerBuilder<Ignore, string>(consumerConfig)
                .SetErrorHandler(LogError)
                .Build();
        }
        
        async void LogError(IConsumer<Ignore, string> consumer, Error error)
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