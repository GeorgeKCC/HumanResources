using LoginContract.Contract;
using LoginContract.Dtos.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Generics.Response;
using Shared.Securities.Models;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(ILogin login, IOptions<TokenConfiguration> options) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var response = await login.LoginAsync(loginRequest, HttpContext);
            return Ok(response);
        }

        [HttpPost("auth-status")]
        [Authorize]
        public IActionResult AuthStatus()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var response = new GenericResponse<bool>("Login success", true);
                return Ok(response);
            }

            return Unauthorized("User is not authenticated");
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete(options.Value.TokenCookieName);
            var response = new GenericResponse<bool>("Logout success", true);
            return Ok(response);
        }
    }
}
