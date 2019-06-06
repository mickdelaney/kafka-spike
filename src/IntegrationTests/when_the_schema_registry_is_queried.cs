using System;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Xunit;

namespace IntegrationTests
{
    public class when_the_schema_registry_is_queried
    {
        [Fact]
        public async Task It_should_list_all_types()
        {
            var cts = new CancellationTokenSource();
            
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // prevent the process from terminating.
                cts.Cancel();
            };

            var config = new KafkaConfig();

            var userConsumer = new DebeziumConsumer
            (
                config: config, 
                cts: cts, 
                name: "UserConsumer",
                topicName: "workforce.recruit.candidate_rates"
            );

            await userConsumer.Consume();
            
        }
    }
}