namespace StressTests.Domain
{
    public class Course
    {
        public int Id { get; }
        public string Title { get; }
        public Presentation BasedOn { get; }

        public Course(int id, string title, Presentation basedOn)
        {
            this.Id = id;
            this.Title = title;
            this.BasedOn = basedOn;
        }
    }
}