using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using ColaboratorContract.Dtos.Response;
using ColaboratorModule.mappers;
using Microsoft.EntityFrameworkCore;
using Shared.Context;
using Shared.Exception;
using Shared.Generics.Response;

namespace ColaboratorModule.Features.CreateColaboratorFeature
{
    internal class CreateColaborator(DatabaseContext colaboratorContext,
                                     IColaboratorRedis colaboratorRedis,
                                     IColaboratorNotificationHub colaboratorNotificationHub) : ICreateColaborator
    {
        public async Task<GenericResponse<ColaboratorDto>> CreateAsync(CreateColaboratorRequest createColaboratorRequest)
        {
            await ValidationExist(createColaboratorRequest);

            var colaborator = createColaboratorRequest.Map();
            colaboratorContext.Add(colaborator);

            await colaboratorContext.SaveChangesAsync();

            var colaboratorDto = colaborator.Map(colaborator.Id);

            await colaboratorRedis.RemoveListAll();

            await colaboratorNotificationHub.NotificationCreateColaborator(colaboratorDto);

            return new GenericResponse<ColaboratorDto>("Create colaborator success", colaboratorDto);
        }

        private async Task ValidationExist(CreateColaboratorRequest createColaboratorRequest)
        {
            var email = createColaboratorRequest.Email.ToLower();
            var documentNumber = createColaboratorRequest.DocumentNumber.ToLower();

            var existEmail = await colaboratorContext.Colaborators
                .AnyAsync(x => x.Email.ToLower() == email);

            if (existEmail)
            {
                throw new ExistColaboratorCustomException("colaborator with this email already exists");
            }

            var existDocumentNumber = await colaboratorContext.Colaborators
                .AnyAsync(x => x.DocumentNumber.ToLower() == documentNumber);

            if (existDocumentNumber)
            {
                throw new ExistColaboratorCustomException("colaborator with this document number already exists");
            }
        }
    }
}