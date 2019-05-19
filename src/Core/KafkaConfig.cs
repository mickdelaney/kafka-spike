namespace Core
{
    public static class KafkaConfig
    {
        public static string Brokers = "192.168.1.160";
        public static int CommitPeriod = 5;
        
        public static string ResumeParseConsumerGroupId = "resume_parse_consumer";
        public static string ResumeTopic = "resume_ingestion";
    }
}