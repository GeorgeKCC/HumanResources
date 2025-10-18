using Shared.Securities.Models;

namespace Shared.Securities.Contracts
{
    public interface IPasswordHashWithSalt
    {
        HashPasswordResponse HashPassword(string password);
        bool VerifyPassword(string password, string storedHash, string storedSalt);
    }
}
