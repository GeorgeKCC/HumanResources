using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Response;
using ManagementContract.Contracts;
using ManagementContract.Dtos.Request;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Hybrid;
using Shared.Context;
using Shared.Entities;
using Shared.Generics.Response;
using Shared.hub;
using Shared.Securities.Contracts;
using Shared.Securities.Models;

namespace ManagementModule.Features.CreateSecurityFeature
{
    internal class CreateSecurity(
                                  DatabaseContext managementContext,
                                  IGetByIdColaborator getByIdColaborator,
                                  IPasswordHashWithSalt passwordHashWithSalt,
                                  IColaboratorNotificationHub colaboratorNotificationHub,
                                  IColaboratorRedis colaboratorRedis
                                 ) : ICreateSecurity
    {
        public async Task<GenericResponse<bool>> CreateAsync(SecurityRequest securityRequest)
        {
            var colaborator = await getByIdColaborator.GetByIdAsync(securityRequest.ColaboratorId);

            if (colaborator.Data.IsActive)
            {
                return new GenericResponse<bool>("Colaborator is active", true);
            }

            var securityPassword = passwordHashWithSalt.HashPassword(colaborator.Data.Name);

            Security security = await SaveChangesAsync(colaborator, securityPassword);

            await colaboratorRedis.RemoveListAll();

            await NotificationHub(colaborator, security);

            return new GenericResponse<bool>("Success create security", true);
        }

        private async Task<Security> SaveChangesAsync(GenericResponse<ColaboratorDto> colaborator, HashPasswordResponse securityPassword)
        {
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
            return security;
        }

        private async Task NotificationHub(GenericResponse<ColaboratorDto> colaborator, Security security)
        {
            var colaboratorHub = new ColaboratorDto(colaborator.Data.Id, colaborator.Data.Name,
                                                    colaborator.Data.LastName, colaborator.Data.Email,
                                                    colaborator.Data.DocumentType, colaborator.Data.DocumentNumber,
                                                    security.Active);

            await colaboratorNotificationHub.NotificationCreateColaborator(colaboratorHub);
        }
    }
}
