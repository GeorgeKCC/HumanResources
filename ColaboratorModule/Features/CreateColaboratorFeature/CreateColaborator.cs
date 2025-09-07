using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using ColaboratorContract.Dtos.Response;
using ColaboratorModule.Data.Context;
using ColaboratorModule.mappers;
using Shared.Generics.Response;

namespace ColaboratorModule.Features.CreateColaboratorFeature
{
    internal class CreateColaborator(ColaboratorContext colaboratorContext) : ICreateColaborator
    {
        public async Task<GenericResponse<ColaboratorDto>> CreateAsync(CreateColaboratorRequest createColaboratorRequest)
        {
            var colaborator = createColaboratorRequest.Map();
            colaboratorContext.Add(colaborator);

            var id = await colaboratorContext.SaveChangesAsync();

            return new GenericResponse<ColaboratorDto>("Create colaborator success", colaborator.Map(id));
        }
    }
}