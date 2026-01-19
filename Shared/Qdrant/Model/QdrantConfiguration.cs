namespace Shared.Qdrant.Model
{
    internal class QdrantConfiguration
    {
        public string Host { get; init; } = "";
        public int GrpcPort { get; init; } = 0;
        public int TimeoutSeconds { get; init; } = 10;
    }
}
