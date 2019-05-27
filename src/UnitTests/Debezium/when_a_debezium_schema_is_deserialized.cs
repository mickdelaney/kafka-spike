using System.IO;
using Core.Debezium;
using Core.Domain.Rates;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace UnitTests.Debezium
{
    public class when_a_debezium_schema_is_deserialized
    {
        
        
        [Fact]
        public void It_should_map_all_properties()
        {
            var dllPath = typeof(when_a_debezium_schema_is_deserialized).Assembly.Location;
            var binFolder = Path.GetDirectoryName(dllPath);
            var pathToFile = Path.Combine(binFolder, "Debezium", "ExampleOutput.json");
            var stream = File.Open(pathToFile, FileMode.Open);
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            var output = JsonConvert.DeserializeObject<DebeziumEnvelope<ApplicationRate>>(json);
            
            output.ShouldNotBeNull();
        }
    }
}