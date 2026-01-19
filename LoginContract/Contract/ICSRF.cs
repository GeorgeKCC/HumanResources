using LoginContract.Dtos.Responses;
using Microsoft.AspNetCore.Http;

namespace LoginContract.Contract
{
    public interface ICSRF
    {
        CSRFDto GetToken(HttpContext httpContext);
    }
}
