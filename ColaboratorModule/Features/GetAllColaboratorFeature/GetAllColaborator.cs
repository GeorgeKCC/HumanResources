using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Response;
using ColaboratorModule.enums;
using ColaboratorModule.mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Shared.Context;
using Shared.Generics.Response;

namespace ColaboratorModule.Features.GetAllColaboratorFeature
{
    internal class GetAllColaborator(DatabaseContext colaboratorContext, HybridCache hybridCache) : IGetAllColaborator
    {
        public async Task<GenericResponse<IEnumerable<ColaboratorDto>>> GetAllAsync()
        {
            var response = await hybridCache.GetOrCreateAsync(
                           ColaboratorKeyRedisEnum.GetAllColaboratorKey,
                            async _ => await GetAllColaborators());
            return response;
        }

        private async Task<GenericResponse<IEnumerable<ColaboratorDto>>> GetAllColaborators()
        {
            var colaborators = await colaboratorContext.Colaborators.AsNoTracking()
                                                                                .Include(x => x.Security)
                                                                                .OrderByDescending(x => x.Id)
                                                                                .ToListAsync();
            var colabortorsMap = colaborators.Map();
            return new GenericResponse<IEnumerable<ColaboratorDto>>("Get all colaborator", colabortorsMap);
        }
    }
}
