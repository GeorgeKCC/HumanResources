using ColaboratorContract.Contracts;
using ColaboratorModule.enums;
using Microsoft.Extensions.Caching.Hybrid;

namespace ColaboratorModule.Features.ColaboratorRedisFeature
{
    internal class ColaboratorRedis(HybridCache hybridCache) : IColaboratorRedis
    {
        public async Task RemoveById(int id)
        {
            await hybridCache.RemoveAsync($"keyColaborator_{id}"); ;
        }

        public async Task RemoveListAll()
        {
            await hybridCache.RemoveAsync(ColaboratorKeyRedisEnum.GetAllColaboratorKey);
        }

        public async Task<T> GetOrCreateAsync<T>(Task<Func<T>> func) where T : class
        {
            var response = await hybridCache.GetOrCreateAsync(
                "keyColaborator",
                async _ =>
                {
                    var f = await func;
                    return await Task.FromResult(f());
                });
            return response;
        }
    }
}
