using ManagementContract.Dtos.Request;
using Shared.Generics.Response;

namespace ManagementContract.Contracts
{
    public interface ICreateSecurity
    {
        Task<GenericResponse<bool>> CreateAsync(SecurityRequest securityRequest);
    }
}
