using System;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Messages;

namespace Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };
            
            var config = new KafkaConfig();
                
            Console.WriteLine($"UserProducer producing on {config.UsersTopic}. Enter user names, Ctrl+C to exit.");

            var cache = new SubjectNameSchemaCache();
            cache.Init(config.UsersTopic);
            
            var userProducer = new UserProducer
            (
                config: config,
                cts: cts,
                name: "UserProducer",
                topicName: config.UsersTopic,
                cache: cache
            );

            await userProducer.Produce();
        }
    }
}