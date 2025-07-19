using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Produtos;

public static class GetProdutoById
{
    public record Query(int Id) : IRequest<ApiResponse<ProdutoDto>>;

    public class Handler : IRequestHandler<Query, ApiResponse<ProdutoDto>>
    {
        private readonly PetShopContext _context;

        public Handler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<ProdutoDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var produto = await _context.Produtos
                .Include(p => p.Categoria)
                .Where(p => p.Id == request.Id && p.Ativo && p.Categoria.Ativo)
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
                .FirstOrDefaultAsync(cancellationToken);

            if (produto == null)
                return ApiResponseExtensions.ToErrorResponse<ProdutoDto>("Produto não encontrado");

            return produto.ToSuccessResponse();
        }
    }
}
