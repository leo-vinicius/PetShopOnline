using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Produtos;

public static class GetProdutos
{
    public record Query(int? CategoriaId = null) : IRequest<ApiResponse<List<ProdutoDto>>>;

    public class Handler : IRequestHandler<Query, ApiResponse<List<ProdutoDto>>>
    {
        private readonly PetShopContext _context;

        public Handler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<ProdutoDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.Ativo && p.Categoria.Ativo);

            if (request.CategoriaId.HasValue)
                query = query.Where(p => p.CategoriaId == request.CategoriaId.Value);

            var produtos = await query
                .Select(p => new ProdutoDto(
                    p.Id,
                    p.Nome,
                    p.Descricao,
                    p.Preco,
                    p.Estoque,
                    p.Ativo,
                    p.ImagemUrl,
                    p.CategoriaId,
                    p.Categoria.Nome
                ))
                .ToListAsync(cancellationToken);

            return produtos.ToSuccessResponse();
        }
    }
}
