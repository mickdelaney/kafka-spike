using System;
using System.Threading;
using System.Threading.Tasks;
using Core;

namespace Consumer
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

            var userConsumer = new UserConsumer
            (
                config: config, 
                cts: cts, 
                name: "UserConsumer",
                topicName: config.UsersTopic
            );

            await userConsumer.Consume();
        }
    }
}