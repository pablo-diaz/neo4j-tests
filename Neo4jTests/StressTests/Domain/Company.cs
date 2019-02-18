using System.Linq;
using System.Collections.Generic;

namespace StressTests.Domain
{
    public class Company
    {
        public int Id { get; }
        public string Name { get; }

        public IEnumerable<User> Users { get; }
        public IEnumerable<User> HeadCoaches { get; }
        public IEnumerable<User> ContentAuthors { get; }
        public IEnumerable<User> CourseAuthors { get; }

        public Company(int id, string name, IEnumerable<User> users, IEnumerable<User> headCoaches, IEnumerable<User> contentAuthors, IEnumerable<User> courseAuthors)
        {
            this.Id = id;
            this.Name = name;

            this.Users = users ?? new List<User>();
            this.HeadCoaches = headCoaches ?? new List<User>();
            this.ContentAuthors = contentAuthors ?? new List<User>();
            this.CourseAuthors = courseAuthors ?? new List<User>();
        }

        public override string ToString()
        {
            return $"[ID: {this.Id}] {this.Name} - [{this.Users.Count()} users]";
        }
    }
}
