using Shared.Entities;

namespace Shared.Securities.Contracts
{
    public interface IGenerateToken
    {
        string CreateToken(string Email, string ColaboratorId);
    }
}
