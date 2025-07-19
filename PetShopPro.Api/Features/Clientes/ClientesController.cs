using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Clientes;

[ApiController]
[Route("api/[controller]")]
[Tags("Clientes")]
public class ClientesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cadastrar novo cliente
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ClienteDto>>> Create([FromBody] CreateClienteRequest request)
    {
        var command = new CreateCliente.Command(
            request.Nome,
            request.Email,
            request.Telefone,
            request.Senha
        );

        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(Create), result);
    }

    /// <summary>
    /// Adicionar endereço ao cliente logado
    /// </summary>
    [HttpPost("enderecos")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<EnderecoDto>>> CreateEndereco([FromBody] CreateEnderecoRequest request)
    {
        var command = new CreateEndereco.Command(
            request.Logradouro,
            request.Numero,
            request.Bairro,
            request.Cidade,
            request.Estado,
            request.CEP,
            User
        );

        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(CreateEndereco), result);
    }

    /// <summary>
    /// Listar endereços do cliente logado
    /// </summary>
    [HttpGet("enderecos")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<List<EnderecoDto>>>> GetMeusEnderecos()
    {
        var query = new GetMeusEnderecos.Query(User);
        var result = await _mediator.Send(query);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Listar todos os clientes (apenas administradores)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ApiResponse<List<ClienteDto>>>> GetTodosClientes()
    {
        var query = new GetTodosClientes.Query();
        var result = await _mediator.Send(query);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
