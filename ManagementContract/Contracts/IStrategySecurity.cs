using ManagementContract.Dtos.Request;
using Shared.Generics.Response;

namespace ManagementContract.Contracts
{
    public interface IStrategySecurity
    {
        string OperationType { get; }

        Task<GenericResponse<bool>> CreateAsync(SecurityRequest securityRequest);
    }
}
