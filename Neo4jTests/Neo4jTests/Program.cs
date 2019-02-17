namespace Neo4jTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration config = new AppSettingsConfiguration();
            using (var neo4jClient = GraphClientUtilities.Create(config))
            {
                neo4jClient.Connect();
                var repo = new Repository(neo4jClient);

                var company = new Company() { Name = "Compañía 01" };
                repo.GetUsersInCompany(company)
                    .Print($"Users in Company '{company.Name}'");

                var user = new User() { Name = "Roger Álvarez" };
                repo.GetChallengesUserHasBeenInvitedInto(user)
                    .Print($"Challenges for User '{user.Name}'");

                repo.GetOtherUsersInvitedToSameChallengesAsUserHasBeenInvitedTo(user)
                    .Print($"Users Invited to Challenges where '{user.Name}' has been invited too");
            }
        }
    }
}
