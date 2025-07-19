using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Categorias;

[ApiController]
[Route("api/[controller]")]
[Tags("Categorias")]
public class CategoriasController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Listar todas as categorias ativas
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CategoriaDto>>>> Get()
    {
        var query = new GetCategorias.Query();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Criar nova categoria (apenas administradores)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ApiResponse<CategoriaDto>>> Create([FromBody] CreateCategoriaRequest request)
    {
        var command = new CreateCategoria.Command(request.Nome);
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(Create), result);
    }
}
