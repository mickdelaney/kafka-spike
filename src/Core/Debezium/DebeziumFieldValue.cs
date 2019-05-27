using Newtonsoft.Json;

namespace Core.Debezium
{
    public class DebeziumFieldValue
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("version")]
        public int Version { get; set; }
        
        [JsonProperty("field")]
        public string Field { get; set; }
    }
}