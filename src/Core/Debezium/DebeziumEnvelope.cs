using Newtonsoft.Json;

namespace Core.Debezium
{
    public class DebeziumEnvelope<T>
    {
        [JsonProperty("schema")]
        public DebeziumSchema Schema { get; set; }
        
        [JsonProperty("payload")]
        public DebeziumPayload<T> Payload { get; set; }
    }
}