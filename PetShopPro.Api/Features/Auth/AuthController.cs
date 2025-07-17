using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Auth;

[ApiController]
[Route("api/[controller]")]
[Tags("Autenticação")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Login de cliente
    /// </summary>
    [HttpPost("cliente/login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> LoginCliente([FromBody] LoginRequest request)
    {
        var command = new LoginCliente.Command(request.Email, request.Senha);
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Login de administrador
    /// </summary>
    [HttpPost("administrador/login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> LoginAdministrador([FromBody] LoginRequest request)
    {
        var command = new LoginAdministrador.Command(request.Email, request.Senha);
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
