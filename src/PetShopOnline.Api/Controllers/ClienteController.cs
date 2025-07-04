using Microsoft.AspNetCore.Mvc;
using PetShopOnline.Application.UseCases.Clientes;
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
        private readonly CreateClienteUseCase _createUseCase;
        private readonly GetClientesUseCase _getClientesUseCase;

        public ClienteController(CreateClienteUseCase createClientesUseCase, GetClientesUseCase getClientesUseCase)
        {
            _createUseCase = createClientesUseCase;
            _getClientesUseCase = getClientesUseCase;
        }

        [HttpPost]
        public IActionResult Create([FromBody] RequestCreateClienteJson request)
        {
            try
            {
                var response = _createUseCase.Executar(request);

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

        [HttpGet]
        public IActionResult GetClientes()
        {
            try
            {
                var response = _getClientesUseCase.Executar();

                return Ok(response);
            }
            catch (NenhumClienteCadastradoException ex)
            {
                return NotFound(new ResponseErrorJson(ex.Message));
            }

        }
    }
}
