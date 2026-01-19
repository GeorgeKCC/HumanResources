namespace Shared.Ollama.Contracts
{
    public interface IOllamaService
    {
        Task<float[]> GenerateEmbeddingAsync(string text);
        Task<string> GenerateResponseAsync(List<string> context, string question);
    }
}
