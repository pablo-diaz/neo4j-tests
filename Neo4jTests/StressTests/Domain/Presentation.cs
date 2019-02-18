using System.Collections.Generic;

namespace StressTests.Domain
{
    public class Presentation
    {
        public int Id { get; }
        public string Title { get; }
        public IEnumerable<Slide> Slides { get; }

        public Presentation(int id, string title, IEnumerable<Slide> slides)
        {
            this.Id = id;
            this.Title = title;

            this.Slides = slides ?? new List<Slide>();
        }
    }
}