using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Pedidos;

[ApiController]
[Route("api/[controller]")]
[Tags("Pedidos")]
[Authorize(Roles = "Cliente")]
public class PedidosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PedidosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Listar pedidos do cliente logado
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<PedidoDto>>>> GetMeusPedidos()
    {
        var query = new GetMeusPedidos.Query(User);
        var result = await _mediator.Send(query);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Confirmar e criar pedido (pagamento simulado e aprovado automaticamente)
    /// </summary>
    [HttpPost("confirmar")]
    public async Task<ActionResult<ApiResponse<PedidoDto>>> ConfirmarPedido([FromBody] CreatePedidoRequest request)
    {
        var command = new CreatePedido.Command(request.EnderecoId, User);
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(ConfirmarPedido), result);
    }
}
