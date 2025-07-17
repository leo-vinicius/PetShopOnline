using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Carrinho;

[ApiController]
[Route("api/[controller]")]
[Tags("Carrinho")]
[Authorize(Roles = "Cliente")]
public class CarrinhoController : ControllerBase
{
    private readonly IMediator _mediator;

    public CarrinhoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obter carrinho do cliente logado
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<CarrinhoDto>>> Get()
    {
        var query = new GetCarrinho.Query(User);
        var result = await _mediator.Send(query);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Adicionar item ao carrinho
    /// </summary>
    [HttpPost("items")]
    public async Task<ActionResult<ApiResponse<CarrinhoDto>>> AdicionarItem([FromBody] AdicionarItemRequest request)
    {
        var command = new AdicionarItemCarrinho.Command(request.ProdutoId, request.Quantidade, User);
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Remover item do carrinho
    /// </summary>
    [HttpDelete("items/{produtoId}")]
    public async Task<ActionResult<ApiResponse<CarrinhoDto>>> RemoverItem(int produtoId)
    {
        var command = new RemoverItemCarrinho.Command(produtoId, User);
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}
