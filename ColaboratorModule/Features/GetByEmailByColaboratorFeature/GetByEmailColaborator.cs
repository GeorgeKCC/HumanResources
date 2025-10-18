using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Response;
using ColaboratorModule.mappers;
using Microsoft.EntityFrameworkCore;
using Shared.Context;
using Shared.Exception;
using Shared.Generics.Response;

namespace ColaboratorModule.Features.GetByEmailByColaboratorFeature
{
    internal class GetByEmailColaborator(DatabaseContext databaseContext) : IGetByEmailColaborator
    {
        public async Task<GenericResponse<ColaboratorDto>> GetByEmailAsync(string email)
        {
            var colaborator = await databaseContext.Colaborators.FirstOrDefaultAsync(x => x.Email == email)
                              ?? throw new ExistColaboratorCustomException("Not exist colaborator");

            return new GenericResponse<ColaboratorDto>("Found colaborator", colaborator.Map(colaborator.Id));
        }
    }
}
