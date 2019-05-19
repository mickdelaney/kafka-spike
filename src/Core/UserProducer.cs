using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;

namespace Core
{
    public class UserProducer
    {
        public async Task Produce()
        {
            
            using (var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig))
            using (var producer = Build(schemaRegistry))
            {
                Console.WriteLine($"{producer.Name} producing on {topicName}. Enter user names, q to exit.");

                int i = 0;
                
                string text;
                while ((text = Console.ReadLine()) != "q")
                {
                    User user = new User { name = text, favorite_color = "green", favorite_number = i++ };
                    
                    await producer
                        .ProduceAsync(topicName, new Message<string, User> { Key = text, Value = user})
                        .ContinueWith(task => task.IsFaulted
                            ? $"error producing message: {task.Exception.Message}"
                            : $"produced to: {task.Result.TopicPartitionOffset}");
                }
            }

            
        }

        static IDisposable Build(CachedSchemaRegistryClient schemaRegistry)
        {
            return new ProducerBuilder<string, User>(producerConfig)
                .SetKeySerializer(new AvroSerializer<string>(schemaRegistry))
                .SetValueSerializer(new AvroSerializer<User>(schemaRegistry))
                .Build();
        }
    }
}