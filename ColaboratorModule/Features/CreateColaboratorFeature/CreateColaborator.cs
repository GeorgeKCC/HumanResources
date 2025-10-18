using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using ColaboratorContract.Dtos.Response;
using ColaboratorModule.mappers;
using Shared.Context;
using Shared.Exception;
using Shared.Generics.Response;

namespace ColaboratorModule.Features.CreateColaboratorFeature
{
    internal class CreateColaborator(DatabaseContext colaboratorContext) : ICreateColaborator
    {
        public async Task<GenericResponse<ColaboratorDto>> CreateAsync(CreateColaboratorRequest createColaboratorRequest)
        {
            var exist = colaboratorContext.Colaborators.Any(x => x.Email == createColaboratorRequest.Email);
            if (exist)
            {
                throw new ExistColaboratorCustomException("colaborator exist");
            }

            var colaborator = createColaboratorRequest.Map();
            colaboratorContext.Add(colaborator);

            var id = await colaboratorContext.SaveChangesAsync();

            return new GenericResponse<ColaboratorDto>("Create colaborator success", colaborator.Map(id));
        }
    }
}