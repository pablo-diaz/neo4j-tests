namespace Neo4jTests
{
    public interface IConfiguration
    {
        string Neo4jURL { get; }
        string Neo4jUserName { get; }
        string Neo4jPassword { get; }
    }
}