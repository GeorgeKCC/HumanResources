using ColaboratorContract.Contracts;
using ColaboratorContract.Dtos.Request;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanResourcesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ColaboratorController(ICreateColaborator createColaborator,
                                       IUpdateColaborator updateColaborator,
                                       IGetAllColaborator getAllColaborator,
                                       IGetByIdColaborator getByIdColaborator,
                                       IAskWithRagColaborator askWithRagColaborator
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
            var response = await getByIdColaborator.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPost("Question")]
        public async Task<IActionResult> AskWithRag(AskQuestionColaboratorRequest askQuestionColaboratorRequest)
        {
            var response = await askWithRagColaborator.Ask(askQuestionColaboratorRequest);
            return Ok(response);
        }
    }
}
