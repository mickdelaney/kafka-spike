using Newtonsoft.Json;

namespace Core.Debezium
{
    public class DebeziumPayload
    {
        [JsonProperty("before")]
        public object Before { get; set; }
        
        [JsonProperty("after")]
        public object After { get; set; }

        [JsonProperty("source")]
        public DebeziumSource Source { get; set; }
        
        [JsonProperty("op")]
        public string Op { get; set; }
        
        [JsonProperty("ts_ms")]
        public long TsMs { get; set; }
    }
}