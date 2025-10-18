using ManagementContract.Contracts;
using ManagementContract.Dtos.Request;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController(ICreateSecurity createSecurity) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(SecurityRequest securityRequest)
        {
            var response = await createSecurity.CreateAsync(securityRequest);
            return Ok(response);
        }
    }
}
