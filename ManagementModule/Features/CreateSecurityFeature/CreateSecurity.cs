using ColaboratorContract.Contracts;
using ManagementContract.Contracts;
using ManagementContract.Dtos.Request;
using Shared.Context;
using Shared.Entities;
using Shared.Generics.Response;
using Shared.Securities.Contracts;

namespace ManagementModule.Features.CreateSecurityFeature
{
    internal class CreateSecurity(
                                  DatabaseContext managementContext,
                                  IGetByIdColaborator getByIdColaborator,
                                  IPasswordHashWithSalt passwordHashWithSalt
                                 ) : ICreateSecurity
    {
        public async Task<GenericResponse<bool>> CreateAsync(SecurityRequest securityRequest)
        {
            var colaborator = await getByIdColaborator.GetByIdAsync(securityRequest.ColaboratorId);
            var securityPassword = passwordHashWithSalt.HashPassword(colaborator.Data.Name);

            var security = new Security()
            {
                Email = colaborator.Data.Email,
                Password = securityPassword.Hash,
                Salt = securityPassword.Salt,
                ColaboratorId = colaborator.Data.Id,
                Active = true
            };

            managementContext.Securities.Add(security);
            await managementContext.SaveChangesAsync();

            return new GenericResponse<bool>("Success create security", true);
        }
    }
}
