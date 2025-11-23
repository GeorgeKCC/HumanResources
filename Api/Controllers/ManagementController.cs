using ManagementContract.Contracts;
using ManagementContract.Dtos.Request;
using ManagementContract.Enums;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Generics.Response;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ManagementController(ICreateActiveDeactive<GenericResponse<bool>> createActiveDeactive) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(SecurityRequest securityRequest,
                                                [FromServices] IAntiforgery antiforgery)
        {
            await antiforgery.ValidateRequestAsync(HttpContext);
            var response = await createActiveDeactive.Execute(securityRequest, ManagementProcessType.CreateOrActiveColaborator);
            return Ok(response);
        }

        [HttpPost("Deactivated")]
        public async Task<IActionResult> Deactivated(SecurityRequest securityRequest,
                                                    [FromServices] IAntiforgery antiforgery)
        {
            await antiforgery.ValidateRequestAsync(HttpContext);
            var response = await createActiveDeactive.Execute(securityRequest, ManagementProcessType.DeactiveColaborator);
            return Ok(response);
        }
    }
}
