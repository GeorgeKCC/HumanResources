using System.Security.Cryptography;

namespace Shared.Securities.Encriptions
{
    internal class PasswordHashWithSalt : IPasswordHashWithSalt
    {
        private const int SALT_SIZE = 16;
        private const int ITERATIONS = 100_000;
        private const int HASH_SIZE = 32;

        public HashPasswordResponse HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(SALT_SIZE);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, ITERATIONS, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(HASH_SIZE);

            string hash = Convert.ToBase64String(hashBytes);
            string salt = Convert.ToBase64String(saltBytes);

            return new HashPasswordResponse()
            {
                Hash = hash,
                Salt = salt
            };
        }

        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, ITERATIONS, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(HASH_SIZE);
            string hash = Convert.ToBase64String(hashBytes);

            return hash == storedHash;
        }
    }
}
