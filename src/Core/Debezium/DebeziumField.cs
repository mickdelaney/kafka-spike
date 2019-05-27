using Newtonsoft.Json;

namespace Core.Debezium
{
    public class DebeziumField
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("optional")]
        public string Optional { get; set; }
        
        [JsonProperty("field")]
        public string Field { get; set; }
        
        [JsonProperty("fields")]
        public DebeziumFieldValue[] Fields { get; set; }
    }
}