using System.Collections.Generic;

using Neo4jClient;

namespace Neo4jTests
{
    public class Repository
    {
        // https://github.com/Readify/Neo4jClient/wiki/cypher

        private readonly IGraphClient _neo4jClient;

        public Repository(IGraphClient neo4jClient)
        {
            this._neo4jClient = neo4jClient;
        }

        public IEnumerable<User> GetUsersInCompany(Company company)
        {
            return this._neo4jClient
                       .Cypher
                       .Match("(c)<-[:WorksAt]-(u)")
                       .Where<Company>(c => c.Name == company.Name)
                       .Return<User>("u")
                       .Results;
        }

        public IEnumerable<Challenge> GetChallengesUserHasBeenInvitedInto(User user)
        {
            return this._neo4jClient
                       .Cypher
                       .Match("(u)-[:ParticipatesIn]->(c)")
                       .Where<User>(u => u.Name == user.Name)
                       .Return<Challenge>("c")
                       .Results;
        }

        public IEnumerable<User> GetOtherUsersInvitedToSameChallengesAsUserHasBeenInvitedTo(User user)
        {
            return this._neo4jClient
                       .Cypher
                       .Match("(u1)-[:ParticipatesIn]->(c)<-[:ParticipatesIn]-(u2)")
                       .Where<User>(u1 => u1.Name == user.Name)
                       .AndWhere("u1.name <> u2.name")
                       .ReturnDistinct<User>("u2")
                       .Results;
        }
    }
}
