using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using Shared.Ollama.Contracts;
using Shared.Qdrant.Repository.Contracts;
using Shared.Qdrant.Tables;

namespace ColaboratorModule.Features.AskWithRagColaboratorFeature
{
    internal class AskWithRagColaborator(IOllamaService ollama,
                                         IEnumerable<IQdrantRepository> qdrantRepository) : IAskWithRagColaborator
    {
        private readonly IQdrantRepository _qdrantRepository = qdrantRepository.FirstOrDefault(x => x.Table == QdrantTable.COLABORATOR_TABLE) ?? throw new Exception();

        public async Task<string> Ask(AskQuestionColaboratorRequest askQuestionColaboratorRequest)
        {
           var embending = await ollama.GenerateEmbeddingAsync(askQuestionColaboratorRequest.Question);
           var context = await _qdrantRepository.SearchAsync(embending);

            var response = await ollama.GenerateResponseAsync(context, askQuestionColaboratorRequest.Question);
            return response;
        }
    }
}
