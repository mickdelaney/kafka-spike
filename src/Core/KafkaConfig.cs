namespace Core
{
    public class KafkaConfig
    {
        public string Brokers { get; set; } = "192.168.1.160";
        public int CommitPeriod { get; set; } = 5;
        public string ResumeParseConsumerGroupId { get; set; } = "resume_parse_consumer";
        public string ResumeTopic { get; set; } = "new_resumes";
        public string SchemaRegistryUrl { get; set; } = "http://192.168.1.160:8081";
        
        public string UsersTopic { get; set; } = "accounts";
    }
}