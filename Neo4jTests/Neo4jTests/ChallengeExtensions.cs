using System;
using System.Linq;
using System.Collections.Generic;

namespace Neo4jTests
{
    public static class ChallengeExtensions
    {
        public static void Print(this IEnumerable<Challenge> challengeList) => challengeList.ToList().ForEach(u => u.Print());

        public static void Print(this IEnumerable<Challenge> challengeList, string title)
        {
            Console.WriteLine("-----------------------");
            Console.WriteLine(title);
            challengeList.Print();
        }

        public static void Print(this Challenge challenge) => Console.WriteLine($"Challenge: {challenge.Name}");
    }
}
