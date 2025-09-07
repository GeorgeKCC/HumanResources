using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColaboratorController(
                                        ICreateColaborator createColaborator,
                                        IUpdateColaborator updateColaborator
                                      ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateColaboratorRequest createColaboratorRequest)
        {
            var response = await createColaborator.CreateAsync(createColaboratorRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateColaboratorRequest updateColaboratorRequest)
        {
            var response = await updateColaborator.UpdateColaboratorAsync(updateColaboratorRequest);
            return Ok(response);
        }
    }
}
