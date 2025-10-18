using LoginContract.Dtos.Requests;
using Microsoft.AspNetCore.Http;
using Shared.Generics.Response;

namespace LoginContract.Contract
{
    public interface ILogin
    {
        Task<GenericResponse<bool>> LoginAsync(LoginRequest loginRequest, HttpContext httpContext);
    }
}
