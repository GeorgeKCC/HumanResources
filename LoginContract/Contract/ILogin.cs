using LoginContract.Dtos.Requests;
using LoginContract.Dtos.Responses;
using Microsoft.AspNetCore.Http;
using Shared.Generics.Response;

namespace LoginContract.Contract
{
    public interface ILogin
    {
        Task<GenericResponse<LoginDto>> LoginAsync(LoginRequest loginRequest, HttpContext httpContext);
    }
}
