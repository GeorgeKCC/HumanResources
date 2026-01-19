namespace Shared.Qdrant.Repository.Contracts
{
    public interface IQdrantRepository
    {
        string Table { get; }

        Task UpsertAsync(Colaborator colaborator, float[] embedding, string content);
        Task<List<string>> SearchAsync(float[] vector);
    }
}
