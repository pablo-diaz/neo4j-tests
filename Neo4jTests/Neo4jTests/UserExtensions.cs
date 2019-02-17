using System;
using System.Linq;
using System.Collections.Generic;

namespace Neo4jTests
{
    public static class UserExtensions
    {
        public static void Print(this IEnumerable<User> userList) => userList.ToList().ForEach(u => u.Print());

        public static void Print(this IEnumerable<User> userList, string title)
        {
            Console.WriteLine("-----------------------");
            Console.WriteLine(title);
            userList.Print();
        }

        public static void Print(this User user) => Console.WriteLine($"Name: {user.Name}");
    }
}
