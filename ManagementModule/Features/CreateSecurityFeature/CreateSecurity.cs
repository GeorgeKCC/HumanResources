using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Response;
using ManagementContract.Contracts;
using ManagementContract.Dtos.Request;
using ManagementContract.Enums;
using Microsoft.EntityFrameworkCore;
using Shared.Context;
using Shared.Entities;
using Shared.Generics.Response;
using Shared.Securities.Contracts;

namespace ManagementModule.Features.CreateSecurityFeature
{
    internal class CreateSecurity(
                                  DatabaseContext managementContext,
                                  IGetByIdColaborator getByIdColaborator,
                                  IPasswordHashWithSalt passwordHashWithSalt,
                                  IColaboratorNotificationHub colaboratorNotificationHub,
                                  IColaboratorRedis colaboratorRedis
                                 ) : IStrategySecurity
    {
        public string OperationType => ManagementProcessType.CreateOrActiveColaborator.ToString();

        public async Task<GenericResponse<bool>> CreateAsync(SecurityRequest securityRequest)
        {
            var colaborator = await getByIdColaborator.GetByIdAsync(securityRequest.ColaboratorId);

            if (colaborator.Data.IsActive)
            {
                return new GenericResponse<bool>("Colaborator is active", true);
            }

            Security? existingSecurity = await GetSecurityByColaboratorId(securityRequest);

            if (existingSecurity != null)
            {
                Security securityUpdate = await UpdateChangeAsync(existingSecurity);

                await RemoveRedisAndNotificationHub(colaborator, securityUpdate);

                return new GenericResponse<bool>("Success active security", true);
            }

            Security security = await SaveChangesAsync(colaborator);

            await RemoveRedisAndNotificationHub(colaborator, security);

            return new GenericResponse<bool>("Success create security", true);
        }

        private async Task<Security?> GetSecurityByColaboratorId(SecurityRequest securityRequest)
        {
            return await managementContext.Securities.AsNoTracking().FirstOrDefaultAsync(s => s.ColaboratorId == securityRequest.ColaboratorId);
        }

        private async Task RemoveRedisAndNotificationHub(GenericResponse<ColaboratorDto> colaborator, Security securityUpdate)
        {
            await colaboratorRedis.RemoveListAll();

            await NotificationHub(colaborator, securityUpdate);
        }

        private async Task<Security> UpdateChangeAsync(Security existingSecurity)
        {
            var securityUpdate = new Security()
            {
                Id = existingSecurity.Id,
                Email = existingSecurity.Email,
                Password = existingSecurity.Password,
                Salt = existingSecurity.Salt,
                ColaboratorId = existingSecurity.ColaboratorId,
                Active = true
            };

            managementContext.Securities.Update(securityUpdate);
            await managementContext.SaveChangesAsync();
            return securityUpdate;
        }

        private async Task<Security> SaveChangesAsync(GenericResponse<ColaboratorDto> colaborator)
        {
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
