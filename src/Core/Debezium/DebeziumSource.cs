using Newtonsoft.Json;

namespace Core.Debezium
{
    public class DebeziumSource
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("version")]
        public string Version { get; set; }
        
        [JsonProperty("field")]
        public string Field { get; set; }
    }
}