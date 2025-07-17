using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Shared.DTOs;
using System.Security.Claims;

namespace PetShopPro.Api.Features.Clientes;

public static class GetMeusEnderecos
{
    public record Query(ClaimsPrincipal User) : IRequest<ApiResponse<List<EnderecoDto>>>;

    public class Handler : IRequestHandler<Query, ApiResponse<List<EnderecoDto>>>
    {
        private readonly PetShopContext _context;

        public Handler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<EnderecoDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userType = request.User.FindFirst("UserType")?.Value;

            if (userType != "Cliente" || !int.TryParse(userIdClaim, out var clienteId))
                return ApiResponseExtensions.ToErrorResponse<List<EnderecoDto>>("Acesso negado");

            var enderecos = await _context.Enderecos
                .Where(e => e.ClienteId == clienteId)
                .Select(e => new EnderecoDto(
                    e.Id,
                    e.Logradouro,
                    e.Numero,
                    e.Bairro,
                    e.Cidade,
                    e.Estado,
                    e.CEP
                ))
                .ToListAsync(cancellationToken);

            return enderecos.ToSuccessResponse();
        }
    }
}
