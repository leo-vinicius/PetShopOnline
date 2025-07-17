using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Administradores;

[ApiController]
[Route("api/[controller]")]
[Tags("Administradores")]
public class AdministradoresController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly PetShopContext _context;

    public AdministradoresController(IMediator mediator, PetShopContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    /// <summary>
    /// Cadastrar novo administrador (primeiro admin não precisa de autenticação, demais precisam)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<AdministradorDto>>> Create([FromBody] CreateAdministradorRequest request)
    {
        // Verifica se já existe algum administrador no sistema
        var hasAdmins = await _context.Administradores.AnyAsync(a => a.Ativo);

        // Se já existem administradores, exige autenticação
        if (hasAdmins)
        {
            // Verifica se o usuário está autenticado como administrador
            if (!User.Identity?.IsAuthenticated == true || !User.IsInRole("Administrador"))
            {
                return Unauthorized(ApiResponseExtensions.ToErrorResponse<AdministradorDto>(
                    "Apenas administradores podem criar novos administradores"));
            }
        }

        var command = new CreateAdministrador.Command(
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
}
