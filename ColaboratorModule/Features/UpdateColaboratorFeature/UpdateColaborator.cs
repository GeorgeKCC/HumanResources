using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using ColaboratorContract.Dtos.Response;
using ColaboratorModule.Data.Context;
using ColaboratorModule.mappers;
using Microsoft.EntityFrameworkCore;
using Shared.Exception;
using Shared.Generics.Response;

namespace ColaboratorModule.Features.UpdateColaboratorFeature
{
    internal class UpdateColaborator(ColaboratorContext colaboratorContext) : IUpdateColaborator
    {
        public async Task<GenericResponse<ColaboratorDto>> UpdateColaboratorAsync(UpdateColaboratorRequest updateColaboratorRequest)
        {
            var colaborator = await colaboratorContext.Colaborators
                                    .FirstOrDefaultAsync(x => x.Id == updateColaboratorRequest.Id)
                                    ?? throw new NotFoundCustomException("Not found colaborator");

            SetColaborator(updateColaboratorRequest, colaborator);

            colaboratorContext.Colaborators.Update(colaborator);
            await colaboratorContext.SaveChangesAsync();

            return new GenericResponse<ColaboratorDto>("Update colaborator success", colaborator.Map(colaborator.Id));
        }

        private static void SetColaborator(UpdateColaboratorRequest updateColaboratorRequest, Shared.Entities.Colaborator colaborator)
        {
            colaborator.Name = updateColaboratorRequest.Name;
            colaborator.LastName = updateColaboratorRequest.LastName;
            colaborator.DocumentNumber = updateColaboratorRequest.DocumentNumber;
            colaborator.DocumentType = updateColaboratorRequest.DocumentType;
        }
    }
}
