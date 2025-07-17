using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Categorias;

public static class GetCategorias
{
    public record Query() : IRequest<ApiResponse<List<CategoriaDto>>>;

    public class Handler : IRequestHandler<Query, ApiResponse<List<CategoriaDto>>>
    {
        private readonly PetShopContext _context;

        public Handler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<CategoriaDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var categorias = await _context.Categorias
                .Where(c => c.Ativo)
                .Select(c => new CategoriaDto(c.Id, c.Nome, c.Ativo))
                .ToListAsync(cancellationToken);

            return categorias.ToSuccessResponse();
        }
    }
}
