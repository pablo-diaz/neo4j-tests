using System.Collections.Generic;

namespace StressTests.Domain
{
    public class Challenge
    {
        public int Id { get; }
        public string Title { get; }

        public IEnumerable<User> Participants { get; }
        public IEnumerable<User> Reviewers { get; }

        public Challenge(int id, string title, IEnumerable<User> participants, IEnumerable<User> reviewers)
        {
            this.Id = id;
            this.Title = title;
            this.Participants = participants ?? new List<User>();
            this.Reviewers = reviewers ?? new List<User>();
        }
    }
}