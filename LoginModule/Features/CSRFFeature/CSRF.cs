using Azure;
using LoginContract.Contract;
using LoginContract.Dtos.Responses;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Shared.Securities.Constants;

namespace LoginModule.Features.CSRFFeature
{
    internal class CSRF(IAntiforgery antiforgery) : ICSRF
    {
        public CSRFDto GetToken(HttpContext httpContext)
        {
            var tokens = antiforgery.GetAndStoreTokens(httpContext);

            if (tokens.CookieToken is null)
            {
                return new CSRFDto(tokens.RequestToken!);
            }

            httpContext.Response.Cookies.Append(
                CSRF_Constant.KEY,
                tokens.CookieToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

            return new CSRFDto(tokens.RequestToken!);
        }
    }
}
