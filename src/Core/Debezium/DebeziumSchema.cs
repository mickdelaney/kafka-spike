using Newtonsoft.Json;

namespace Core.Debezium
{
    public class DebeziumSchema
    {
        [JsonProperty("type")]
        public string Type { get; set; }
         
        [JsonProperty("fields")]
        public DebeziumField[] Fields { get; set; }

        [JsonProperty("optional")]
        public bool Optional { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}