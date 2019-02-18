using System.Configuration;

namespace StressTests.Infrastructure
{
    public class AppSettingsConfiguration : IConfiguration
    {
        public string Neo4jURL => ConfigurationManager.AppSettings["Neo4j.URL"];

        public string Neo4jUserName => ConfigurationManager.AppSettings["Neo4j.UserName"];

        public string Neo4jPassword => ConfigurationManager.AppSettings["Neo4j.Password"];
    }
}