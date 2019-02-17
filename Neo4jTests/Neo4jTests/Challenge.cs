using Newtonsoft.Json;

namespace Neo4jTests
{
    public class Challenge
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
