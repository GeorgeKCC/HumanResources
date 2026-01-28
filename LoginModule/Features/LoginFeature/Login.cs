using LoginContract.Contract;
using LoginContract.Dtos.Requests;
using LoginContract.Dtos.Responses;
using ManagementContract.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Generics.Response;
using Shared.Securities.Contracts;
using System.Diagnostics;

namespace LoginModule.Features.LoginFeature
{
    internal class Login(
                             ILogger<Login> logger,
                             IGetByEmailSecurity getByEmailSecurity,
                             IPasswordHashWithSalt passwordHashWithSalt,
                             IGenerateToken generateToken,
                             ITokensInsideCookie tokensInsideCookie
                            ) : ILogin
    {
        public async Task<GenericResponse<LoginDto>> LoginAsync(LoginRequest loginRequest, HttpContext httpContext)
        {
            var security = await getByEmailSecurity.GetByEmailAsync(loginRequest.Username);
            var isPasswordCorrect = passwordHashWithSalt.VerifyPassword(loginRequest.Password, security.Password, security.Salt);
            if (isPasswordCorrect is false)
            {
                logger.LogWarning("Failed login attempt for user {Username} at {Timestamp}", loginRequest.Username, DateTime.UtcNow);
                throw new UnauthorizedAccessException();
            }

            logger.LogInformation("User {Username} logged in successfully at {Timestamp}", loginRequest.Username, DateTime.UtcNow);
            var token = generateToken.CreateToken(security.Email, security.ColaboratorId.ToString());
            tokensInsideCookie.SetTokensInsideCookie(token, httpContext);
            return new GenericResponse<LoginDto>("Login success", new LoginDto(security.Email));
        }
    }
}
