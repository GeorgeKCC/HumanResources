using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Securities.Contracts;
using Shared.Securities.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shared.Securities.Token
{
    internal class GenerateToken(IOptions<TokenConfiguration> options) : IGenerateToken
    {
        private readonly TokenConfiguration _tokenConfiguration = options.Value;

        public string CreateToken(string Email, string ColaboratorId)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, Email),
                new("Id", ColaboratorId),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.SecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
               issuer: _tokenConfiguration.Issuer,
               audience: _tokenConfiguration.Audience,
               claims: claims,
               expires: DateTime.UtcNow.AddDays(Convert.ToInt32(_tokenConfiguration.ExpireDay)),
               signingCredentials: creds
           );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
