using Microsoft.AspNetCore.Mvc;
using PetShopOnline.Application.UseCases.Clientes.Create;
using PetShopOnline.Communication.Requests;
using PetShopOnline.Communication.Responses;

namespace PetShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create([FromBody] RequestCreateClienteJson request)
        {
            try
            {
                var useCase = new CreateClienteUseCase();
                var response = useCase.Executar(request);

                return Created(string.Empty, response);
            }
            catch (ArgumentException ex)
            {
                var errorResponse = new ResponseErrorJson(ex.Message);

                return BadRequest(errorResponse);
            }
            catch
            {
                var errorResponse = new ResponseErrorJson("Erro desconhecido");

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
