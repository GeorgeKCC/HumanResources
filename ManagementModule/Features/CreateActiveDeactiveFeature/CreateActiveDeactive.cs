using ManagementContract.Contracts;
using ManagementContract.Dtos.Request;
using ManagementContract.Enums;
using Shared.Generics.Response;

namespace ManagementModule.Features.CreateActiveDeactiveFeature
{
    internal class CreateActiveDeactive(IEnumerable<IStrategySecurity> createSecurity) : ICreateActiveDeactive<GenericResponse<bool>>
    {
        public async Task<GenericResponse<bool>> Execute(SecurityRequest securityRequest, ManagementProcessType managementProcessType)
        {
            if (managementProcessType is ManagementProcessType.CreateOrActiveColaborator 
                                      or ManagementProcessType.DeactiveColaborator)
            {
                var operation = GetStrategy(managementProcessType);
                return await operation.CreateAsync(securityRequest);
            }

            return new GenericResponse<bool>("Invalid Management Process Type", false);
        }

        private IStrategySecurity GetStrategy(ManagementProcessType managementProcessType)
        {
            return createSecurity.FirstOrDefault(x => x.OperationType == managementProcessType.ToString()) ?? throw new Exception();
        }
    }
}
