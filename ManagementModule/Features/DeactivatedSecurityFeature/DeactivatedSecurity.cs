using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Response;
using ManagementContract.Contracts;
using ManagementContract.Dtos.Request;
using ManagementContract.Enums;
using Shared.Context;
using Shared.Entities;
using Shared.Generics.Response;

namespace ManagementModule.Features.DeactivatedSecurityFeature
{
    internal class DeactivatedSecurity(DatabaseContext managementContext,
                                       IGetByIdColaborator getByIdColaborator,
                                       IGetByEmailSecurity getByEmailSecurity,
                                       IColaboratorNotificationHub colaboratorNotificationHub,
                                       IColaboratorRedis colaboratorRedis) : IStrategySecurity
    {
        public string OperationType => ManagementProcessType.DeactiveColaborator.ToString();

        public async Task<GenericResponse<bool>> CreateAsync(SecurityRequest securityRequest)
        {
            var colaborator = await getByIdColaborator.GetByIdAsync(securityRequest.ColaboratorId);
            var security = await getByEmailSecurity.GetByEmailAsync(colaborator.Data.Email);

            Security securityEntity = await UpdateSaveChangeAsync(security);

            await colaboratorRedis.RemoveListAll();

            await NotificationHub(colaborator, securityEntity);

            return new GenericResponse<bool>("Success update security", true);
        }

        private async Task<Security> UpdateSaveChangeAsync(Security security)
        {
            var securityEntity = new Security()
            {
                Active = false,
                Email = security.Email,
                ColaboratorId = security.ColaboratorId,
                Id = security.Id,
                Password = security.Password,
                Salt = security.Salt
            };

            managementContext.Securities.Update(securityEntity);
            await managementContext.SaveChangesAsync();
            return securityEntity;
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
