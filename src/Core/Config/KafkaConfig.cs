namespace Core
{
    public class KafkaConfig
    {
        public string Brokers { get; set; } = "elevate.kafka.local";
        public int CommitPeriod { get; set; } = 5;
        public string SchemaRegistryUrl { get; set; } = "http://elevate.kafka.local:8081";
        
        public string UserConsumerGroupId { get; set; } = "resume_parse_consumer";
        public string UsersTopic { get; set; } = "accounts";
    }
}