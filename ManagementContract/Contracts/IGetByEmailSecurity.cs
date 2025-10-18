using Shared.Entities;

namespace ManagementContract.Contracts
{
    public interface IGetByEmailSecurity
    {
        Task<Security> GetByEmailAsync(string email);
    }
}
