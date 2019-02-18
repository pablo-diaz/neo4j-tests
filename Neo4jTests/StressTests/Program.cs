using System;

using StressTests.Infrastructure;
using StressTests.Utils;

namespace StressTests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var companies = Seeder.CreateCompanies();

            IConfiguration config = new AppSettingsConfiguration();
            using (var neo4jClient = GraphClientUtilities.Create(config))
            {
                neo4jClient.Connect();
                var repo = new Repository(neo4jClient);

                repo.Save(companies, LogIntoConsole, AskToContinueWithNextCompany);
            }
        }

        private static void LogIntoConsole(string messageToLog)
        {
            Console.WriteLine(messageToLog);
        }

        private static bool AskToContinueWithNextCompany()
        {
            Console.Write("Press Y to continue with the next Company: ");
            bool shouldContinue = Console.ReadKey().Key == ConsoleKey.Y;
            Console.WriteLine();
            return shouldContinue;
        }
    }
}
