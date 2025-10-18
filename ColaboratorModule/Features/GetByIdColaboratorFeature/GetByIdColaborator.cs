using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Response;
using ColaboratorModule.mappers;
using Microsoft.EntityFrameworkCore;
using Shared.Context;
using Shared.Exception;
using Shared.Generics.Response;

namespace ColaboratorModule.Features.GetByIdColaboratorFeature
{
    internal class GetByIdColaborator(DatabaseContext colaboratorContext) : IGetByIdColaborator
    {
        public async Task<GenericResponse<ColaboratorDto>> GetByIdAsync(int id)
        {
            var colaborator = await colaboratorContext.Colaborators.FirstOrDefaultAsync(x => x.Id == id)
                              ?? throw new NotFoundCustomException("Not exist colaborator");

            return new GenericResponse<ColaboratorDto>("Found colaborator", colaborator.Map(colaborator.Id));
        }
    }
}
