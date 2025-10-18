using Microsoft.AspNetCore.Http;

namespace Shared.Securities.Contracts
{
    public interface ITokensInsideCookie
    {
        void SetTokensInsideCookie(string token, HttpContext context);
    }
}
