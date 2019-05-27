using Newtonsoft.Json;

namespace Core.Debezium
{
    public class DebeziumPayload<T>
    {
        [JsonProperty("before")]
        public T Before { get; set; }
        
        [JsonProperty("after")]
        public T After { get; set; }

        [JsonProperty("source")]
        public DebeziumSource Source { get; set; }
        
        [JsonProperty("op")]
        public string Op { get; set; }
        
        [JsonProperty("ts_ms")]
        public long TsMs { get; set; }
    }
}