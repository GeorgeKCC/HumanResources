using ManagementContract.Dtos.Request;
using ManagementContract.Enums;

namespace ManagementContract.Contracts
{
    public interface ICreateActiveDeactive<T> where T : class
    {
        Task<T> Execute(SecurityRequest securityRequest, ManagementProcessType managementProcessType);
    }
}
