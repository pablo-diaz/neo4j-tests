namespace StressTests.Domain
{
    public class Slide
    {
        public int Id { get; }
        public string Title { get; }
        public int Order { get; }

        public Slide(int id, string title, int order)
        {
            this.Id = id;
            this.Title = title;
            this.Order = order;
        }
    }
}