using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Core;
using Newtonsoft.Json;

namespace Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var producerConfig = new ProducerConfig { BootstrapServers = KafkaConfig.Brokers };
            
            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                for (var i = 0; i < 10; i++)
                {
                    var id = Guid.NewGuid();
                    
                    var resume = new ResumeMessage { Id = id, Content = $"Resume: Index={i}. Id={id}" };

                    var message = new Message<Null, string> { Value = JsonConvert.SerializeObject(resume) };
                    
                    var result = await producer.ProduceAsync(KafkaConfig.ResumeTopic, message);

                    Console.WriteLine($"Resume {i} sent on Partition: {result.Partition} with Offset: {result.Offset}");
                }
            }

            Console.Read();
        }
    }
}