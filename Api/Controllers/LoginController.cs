using LoginContract.Contract;
using LoginContract.Dtos.Requests;
using LoginContract.Dtos.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Generics.Response;
using Shared.Securities.Constants;
using Shared.Securities.Models;
using System.Security.Claims;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(ILogin login, 
                                 IOptions<TokenConfiguration> options,
                                 ICSRF csrf) : ControllerBase
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
                var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!;
                var response = new GenericResponse<LoginDto>("Login success", new LoginDto(email.Value));
                return Ok(response);
            }

            return Unauthorized("User is not authenticated");
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete(options.Value.TokenCookieName);
            HttpContext.Response.Cookies.Delete(CSRF_Constant.KEY);
            var response = new GenericResponse<bool>("Logout success", true);
            return Ok(response);
        }

        [HttpGet("security/csrf")]
        [Authorize]
        public IActionResult CrsfToken()
        {
            var response = csrf.GetToken();
            return Ok(response);
        }
    }
}
