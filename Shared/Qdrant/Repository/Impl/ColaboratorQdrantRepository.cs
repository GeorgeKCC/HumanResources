using Elastic.CommonSchema;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using Shared.Qdrant.Repository.Contracts;
using Shared.Qdrant.Tables;

namespace Shared.Qdrant.Repository.Impl
{
    internal class ColaboratorQdrantRepository : IQdrantRepository
    {
        private readonly QdrantClient _qdrantClient;
        public string Table => QdrantTable.COLABORATOR_TABLE;

        public ColaboratorQdrantRepository(QdrantClient qdrantClient)
        {
            _qdrantClient = qdrantClient;
            EnsureCollection().GetAwaiter().GetResult();
        }
        public async Task EnsureCollection()
        {
            if (!await _qdrantClient.CollectionExistsAsync(QdrantTable.COLABORATOR_TABLE))
            {
                await _qdrantClient.CreateCollectionAsync(QdrantTable.COLABORATOR_TABLE, new VectorParams
                {
                    Size = 768,
                    Distance = Distance.Cosine
                });
            }
        }

        public async Task UpsertAsync(Colaborator colaborator, float[] embedding, string content)
        {
            await _qdrantClient.UpsertAsync(QdrantTable.COLABORATOR_TABLE, points:
                                     [
                                         new PointStruct()
                                         {
                                             Id = (ulong)colaborator.Id,
                                             Vectors = embedding,
                                             Payload =
                                             {
                                                ["content"] = content,
                                                ["name"] = colaborator.Name,
                                                ["lastName"] = colaborator.LastName,
                                                ["email"] = colaborator.Email
                                             }
                                         }
                                     ]);
        }

        public async Task<List<string>> SearchAsync(float[] vector)
        {
            var result = await _qdrantClient.SearchAsync(QdrantTable.COLABORATOR_TABLE, vector);
            return [.. result.Select(r => r.Payload["content"].StringValue)];
        }
    }
}
