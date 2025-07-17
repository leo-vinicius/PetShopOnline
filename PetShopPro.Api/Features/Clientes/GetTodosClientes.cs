using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Clientes;

public static class GetTodosClientes
{
    public record Query() : IRequest<ApiResponse<List<ClienteDto>>>;

    public class Handler : IRequestHandler<Query, ApiResponse<List<ClienteDto>>>
    {
        private readonly PetShopContext _context;

        public Handler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<ClienteDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var clientes = await _context.Clientes
                .Where(c => c.Ativo)
                .OrderBy(c => c.Nome)
                .Select(c => new ClienteDto(
                    c.Id,
                    c.Nome,
                    c.Email,
                    c.Telefone,
                    c.Ativo
                ))
                .ToListAsync(cancellationToken);

            return clientes.ToSuccessResponse($"{clientes.Count} clientes encontrados");
        }
    }
}
