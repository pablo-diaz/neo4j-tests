namespace StressTests.Utils
{
    public static class IntegerExtensions
    {
        public static string GetName(this int id, string pattern) => $"{pattern}{id}";
    }
}
