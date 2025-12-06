using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Response;
using ColaboratorModule.mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Shared.Context;
using Shared.Exception;
using Shared.Generics.Response;

namespace ColaboratorModule.Features.GetByIdColaboratorFeature
{
    internal class GetByIdColaborator(DatabaseContext colaboratorContext, HybridCache hybridCache) : IGetByIdColaborator
    {
        public async Task<GenericResponse<ColaboratorDto>> GetByIdAsync(int id)
        {
            var result = await hybridCache.GetOrCreateAsync($"keyColaborator_{id}",
                                                            async _ => await GetColaboratorByIdAsync(id));

            return result;
        }

        private async Task<GenericResponse<ColaboratorDto>> GetColaboratorByIdAsync(int id)
        {
            var colaborator = await colaboratorContext.Colaborators.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id)
                                          ?? throw new NotFoundCustomException("Not exist colaborator");

            return new GenericResponse<ColaboratorDto>("Found colaborator", colaborator.Map(colaborator.Id));
        }
    }
}
