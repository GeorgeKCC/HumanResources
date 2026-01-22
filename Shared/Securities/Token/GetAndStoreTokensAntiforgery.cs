

namespace Shared.Securities.Token
{
    internal class GetAndStoreTokensAntiforgery(IAntiforgery antiforgery,
                                                IHttpContextAccessor httpContextAccessor) : IGetAndStoreTokensAntiforgery
    {
        public string GetToken()
        {
            var httpContext = httpContextAccessor.HttpContext ?? throw new Ex("Not fount HttpContext");

            var tokens = antiforgery.GetAndStoreTokens(httpContext);
            var requestToken = tokens.RequestToken ?? throw new Ex("Error generate RequestToken antiforgery");

            if (tokens.CookieToken is null)
            {
                return requestToken;
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

            return requestToken;
        }
    }
}
