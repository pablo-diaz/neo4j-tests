using Newtonsoft.Json;

namespace Neo4jTests
{
    public class Company
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
