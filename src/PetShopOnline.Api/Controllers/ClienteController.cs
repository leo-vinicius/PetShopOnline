using Microsoft.AspNetCore.Mvc;
using PetShopOnline.Application.UseCases.Clientes.Create;
using PetShopOnline.Communication.Requests;
using PetShopOnline.Communication.Responses;
using PetShopOnline.Exceptions;

namespace PetShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly CreateClienteUseCase _useCase;

        public ClienteController(CreateClienteUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpPost]
        public IActionResult Create([FromBody] RequestCreateClienteJson request)
        {
            try
            {
                var response = _useCase.Executar(request);

                return Created(string.Empty, response);
            }
            catch (ClienteJaCadastradoException ex)
            {
                return Conflict(new ResponseErrorJson(ex.Message));
            }
            catch
            {
                var errorResponse = new ResponseErrorJson("Erro desconhecido");

                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }
}
