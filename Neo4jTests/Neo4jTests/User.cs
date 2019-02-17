using Newtonsoft.Json;

namespace Neo4jTests
{
    public class User
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
