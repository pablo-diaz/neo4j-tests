using System.Linq;
using System.Collections.Generic;

namespace StressTests.Domain
{
    public class User
    {
        public int Id { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public string Login { get; }
        public User Manager { get; private set; }

        public IEnumerable<Presentation> AuthorOfContent { get; }
        public IEnumerable<Course> AuthorOfCourses { get; }
        public IEnumerable<Challenge> AuthorOfChallenges { get; }

        public User(int id, string firstname, string lastname, string login, User manager, 
                    IEnumerable<Presentation> authorOfContent, IEnumerable<Course> authorOfCourses, IEnumerable<Challenge> authorOfChallenges)
        {
            this.Id = id;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Login = login;
            this.Manager = manager;

            this.AuthorOfContent = authorOfContent ?? new List<Presentation>();
            this.AuthorOfCourses = authorOfCourses ?? new List<Course>();
            this.AuthorOfChallenges = authorOfChallenges ?? new List<Challenge>();
        }

        public override string ToString()
        {
            return $"[ID: {this.Id}] {this.Firstname} {this.Lastname}";
        }
    }

    public static class UserExtensions
    {
        public static User AssignManager(this User user, User manager) => 
            new User(user.Id, user.Firstname, user.Lastname, user.Login, manager, user.AuthorOfContent, user.AuthorOfCourses, user.AuthorOfChallenges);

        public static User AddChallenges(this User user, IEnumerable<Challenge> challenges)
        {
            var newChallengeList = user.AuthorOfChallenges.Concat(challenges).ToList();
            return new User(user.Id, user.Firstname, user.Lastname, user.Login, user.Manager, user.AuthorOfContent, user.AuthorOfCourses, newChallengeList);
        }

        public static User AddPresentations(this User user, IEnumerable<Presentation> presentations)
        {
            var newPresentationList = user.AuthorOfContent.Concat(presentations).ToList();
            return new User(user.Id, user.Firstname, user.Lastname, user.Login, user.Manager, newPresentationList, user.AuthorOfCourses, user.AuthorOfChallenges);
        }

        public static User AddCourses(this User user, IEnumerable<Course> courses)
        {
            var newCourseList = user.AuthorOfCourses.Concat(courses).ToList();
            return new User(user.Id, user.Firstname, user.Lastname, user.Login, user.Manager, user.AuthorOfContent, newCourseList, user.AuthorOfChallenges);
        }
    }
}