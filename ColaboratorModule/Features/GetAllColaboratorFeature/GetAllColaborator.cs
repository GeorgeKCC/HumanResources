using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Response;
using ColaboratorModule.Data.Context;
using ColaboratorModule.mappers;
using Microsoft.EntityFrameworkCore;
using Shared.Generics.Response;

namespace ColaboratorModule.Features.GetAllColaboratorFeature
{
    internal class GetAllColaborator(ColaboratorContext colaboratorContext) : IGetAllColaborator
    {
        public async Task<GenericResponse<IEnumerable<ColaboratorDto>>> GetAllAsync()
        {
            var colaborators = await colaboratorContext.Colaborators.ToListAsync();
            var colabortorsMap = colaborators.Map();
            return new GenericResponse<IEnumerable<ColaboratorDto>>("Get all colaborator", colabortorsMap);
        }
    }
}
