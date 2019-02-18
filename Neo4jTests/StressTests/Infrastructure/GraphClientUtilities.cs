using System;
using Neo4jClient;

namespace StressTests.Infrastructure
{
    public static class GraphClientUtilities
    {
        public static IGraphClient Create(IConfiguration configuration)
        {
            var user = configuration.Neo4jUserName;
            var password = configuration.Neo4jPassword;
            var neo4jserver = new Uri(configuration.Neo4jURL);
            return new GraphClient(neo4jserver, user, password);
        }
    }
}
