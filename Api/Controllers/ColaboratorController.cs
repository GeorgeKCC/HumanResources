using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using Microsoft.AspNetCore.Antiforgery;
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
                                        IGetAllColaborator getAllColaborator,
                                        IGetByIdColaborator getByIdColaborator
                                      ) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateColaboratorRequest createColaboratorRequest,
                                                [FromServices] IAntiforgery antiforgery)
        {
            await antiforgery.ValidateRequestAsync(HttpContext);
            var response = await createColaborator.CreateAsync(createColaboratorRequest);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateColaboratorRequest updateColaboratorRequest,
                                                int id,
                                                [FromServices] IAntiforgery antiforgery)
        {
            await antiforgery.ValidateRequestAsync(HttpContext);
            var response = await updateColaborator.UpdateColaboratorAsync(updateColaboratorRequest, id);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await getAllColaborator.GetAllAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await hybridCache.GetOrCreateAsync(
                           $"keyColaborator_{id}",
                            async _ => await getByIdColaborator.GetByIdAsync(id));
            return Ok(response);
        }
    }
}
