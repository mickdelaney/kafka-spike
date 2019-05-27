using System;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Xunit;

namespace IntegrationTests
{
    public class when_the_debezium_consumer_connects
    {
        [Fact]
        public async Task It_should_deserialize_the_envelope_correctly()
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