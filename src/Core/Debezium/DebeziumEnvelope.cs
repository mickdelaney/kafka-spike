using Newtonsoft.Json;

namespace Core.Debezium
{
    public class DebeziumEnvelope
    {
        [JsonProperty("schema")]
        public DebeziumSchema Schema { get; set; }
        
        [JsonProperty("payload")]
        public DebeziumPayload Payload { get; set; }
    }
}