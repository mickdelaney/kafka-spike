using System;
using Newtonsoft.Json;

namespace Core
{
    public class ResumeMessage
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}