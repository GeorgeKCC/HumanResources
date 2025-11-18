using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using ColaboratorContract.Dtos.Response;
using ColaboratorModule.mappers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shared.Context;
using Shared.Entities;
using Shared.Exception;
using Shared.Generics.Response;

namespace ColaboratorModule.Features.UpdateColaboratorFeature
{
    internal class UpdateColaborator(
                                     DatabaseContext colaboratorContext,
                                     IValidator<UpdateColaboratorRequest> validator,
                                     IColaboratorRedis colaboratorRedis
                                    ) : IUpdateColaborator
    {
        public async Task<GenericResponse<ColaboratorDto>> UpdateColaboratorAsync(UpdateColaboratorRequest updateColaboratorRequest, int id)
        {
            await ValidateRequest(updateColaboratorRequest);

            var colaborator = await colaboratorContext.Colaborators
                                    .FirstOrDefaultAsync(x => x.Id == id)
                                    ?? throw new NotFoundCustomException("Not found colaborator");

            if(IsIdempotent(updateColaboratorRequest, colaborator))
            {
                return new GenericResponse<ColaboratorDto>("No changes detected", colaborator.Map(colaborator.Id));
            }

            var colaboratorMap = updateColaboratorRequest.Map(colaborator);

            colaboratorContext.Colaborators.Update(colaboratorMap);
            await colaboratorContext.SaveChangesAsync();

            await colaboratorRedis.RemoveById(colaborator.Id);

            return new GenericResponse<ColaboratorDto>("Update colaborator success", colaboratorMap.Map(colaborator.Id));
        }

        private static bool IsIdempotent(UpdateColaboratorRequest updateColaboratorRequest, Colaborator colaborator)
        {
            return string.Equals(colaborator.Name, updateColaboratorRequest.Name, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(colaborator.LastName, updateColaboratorRequest.LastName, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(colaborator.DocumentNumber, updateColaboratorRequest.DocumentNumber, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(colaborator.DocumentType, updateColaboratorRequest.DocumentType, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(colaborator.Email, updateColaboratorRequest.Email, StringComparison.OrdinalIgnoreCase);
        }

        private async Task ValidateRequest(UpdateColaboratorRequest updateColaboratorRequest)
        {
            var validate = await validator.ValidateAsync(updateColaboratorRequest);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }
        }
    }

    internal class UpdateColaboratorValidator : AbstractValidator<UpdateColaboratorRequest>
    {
        public UpdateColaboratorValidator() 
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
            RuleFor(x => x.DocumentNumber).NotNull().NotEmpty();
            RuleFor(x => x.DocumentType).NotNull().NotEmpty();
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
        }
    }
}
