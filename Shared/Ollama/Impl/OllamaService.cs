using Shared.Ollama.Contracts;
using System.Net.Http.Json;

namespace Shared.Ollama.Impl
{
    internal class OllamaService(HttpClient http) : IOllamaService
    {
        private readonly HttpClient _http = http;

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            var response = await _http.PostAsJsonAsync("/api/embeddings", new
            {
                model = "nomic-embed-text",
                prompt = text
            });


            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>();
            return result!.Embedding;
        }

        public async Task<string> GenerateResponseAsync(List<string> context, string question)
        {
            var prompt = $"""
                            Eres un asistente que responde únicamente usando el CONTEXTO proporcionado.

                            REGLAS:
                            - Usa SOLO la información del CONTEXTO.
                            - NO infieras, NO asumas, NO completes información.
                            - Si la respuesta NO está explícitamente en el CONTEXTO, responde exactamente:
                              "No tengo esa información."
                            - Si la respuesta está explícitamente en el CONTEXTO, response amablemente con:
                              "Te comparto la siguiente información"

                            CONTEXTO:
                            {string.Join("\n", context)}

                            PREGUNTA:
                            {question}

                            RESPUESTA:
                          """;


            var response = await _http.PostAsJsonAsync("api/generate", new
            {
                model = "llama3",
                prompt,
                stream = false
            });

            var result = await response.Content.ReadFromJsonAsync<OllamaGenerateResponse>();
            return result!.Response;
        }

        private record OllamaEmbeddingResponse(float[] Embedding);
        private record OllamaGenerateResponse(string Response);
    }
}
