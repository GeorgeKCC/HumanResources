using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ColaboratorController(
                                        HybridCache hybridCache,
                                        ICreateColaborator createColaborator,
                                        IUpdateColaborator updateColaborator,
                                        IGetAllColaborator getAllColaborator
                                      ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateColaboratorRequest createColaboratorRequest)
        {
            var response =  await createColaborator.CreateAsync(createColaboratorRequest);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateColaboratorRequest updateColaboratorRequest, int id)
        {
            var response = await updateColaborator.UpdateColaboratorAsync(updateColaboratorRequest, id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await hybridCache.GetOrCreateAsync(
                           "keyColaborator",
                            async _ => await getAllColaborator.GetAllAsync());
            return Ok(response);
        }
    }
}
