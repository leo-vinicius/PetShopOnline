using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Produtos;

[ApiController]
[Route("api/[controller]")]
[Tags("Produtos")]
public class ProdutosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProdutosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Listar produtos ativos, opcionalmente filtrados por categoria
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ProdutoDto>>>> Get([FromQuery] int? categoriaId = null)
    {
        var query = new GetProdutos.Query(categoriaId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Buscar produto por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProdutoDto>>> GetById(int id)
    {
        var query = new GetProdutoById.Query(id);
        var result = await _mediator.Send(query);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// Criar novo produto (apenas administradores)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ApiResponse<ProdutoDto>>> Create([FromBody] CreateProdutoRequest request)
    {
        var command = new CreateProduto.Command(
            request.Nome,
            request.Descricao,
            request.Preco,
            request.Estoque,
            request.ImagemUrl,
            request.CategoriaId
        );

        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
    }
}
