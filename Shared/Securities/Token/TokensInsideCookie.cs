using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Securities.Contracts;
using Shared.Securities.Models;

namespace Shared.Securities.Token
{
    internal class TokensInsideCookie(IOptions<TokenConfiguration> options) : ITokensInsideCookie
    {
        private readonly TokenConfiguration _tokenConfiguration = options.Value;
        public void SetTokensInsideCookie(string token, HttpContext context)
        {
            context.Response.Cookies.Append(_tokenConfiguration.TokenCookieName, token,
               new CookieOptions
               {
                   Expires = DateTime.UtcNow.AddDays(1),
                   HttpOnly = true,
                   IsEssential = true,
                   Secure = true,
                   SameSite = SameSiteMode.None
               });
        }
    }
}
