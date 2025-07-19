using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Infrastructure.Entities;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Produtos;

public static class CreateProduto
{
    public record Command(
        string Nome,
        string? Descricao,
        decimal Preco,
        int Estoque,
        string? ImagemUrl,
        int CategoriaId
    ) : IRequest<ApiResponse<ProdutoDto>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");

            RuleFor(x => x.Preco)
                .GreaterThan(0).WithMessage("Preço deve ser maior que zero");

            RuleFor(x => x.Estoque)
                .GreaterThanOrEqualTo(0).WithMessage("Estoque deve ser maior ou igual a zero");

            RuleFor(x => x.CategoriaId)
                .GreaterThan(0).WithMessage("Categoria é obrigatória");

            RuleFor(x => x.Descricao)
                .MaximumLength(1000).WithMessage("Descrição deve ter no máximo 1000 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Descricao));

            RuleFor(x => x.ImagemUrl)
                .MaximumLength(2000).WithMessage("URL da imagem deve ter no máximo 2000 caracteres")
                .When(x => !string.IsNullOrEmpty(x.ImagemUrl));
        }
    }

    public class Handler : IRequestHandler<Command, ApiResponse<ProdutoDto>>
    {
        private readonly PetShopContext _context;

        public Handler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<ProdutoDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            // Verifica se a categoria existe e está ativa
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == request.CategoriaId && c.Ativo, cancellationToken);

            if (categoria == null)
                return ApiResponseExtensions.ToErrorResponse<ProdutoDto>("Categoria não encontrada");

            var produto = new Produto
            {
                Nome = request.Nome,
                Descricao = request.Descricao,
                Preco = request.Preco,
                Estoque = request.Estoque,
                ImagemUrl = request.ImagemUrl,
                CategoriaId = request.CategoriaId,
                Ativo = true
            };

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync(cancellationToken);

            var produtoDto = new ProdutoDto(
                produto.Id,
                produto.Nome,
                produto.Descricao,
                produto.Preco,
                produto.Estoque,
                produto.Ativo,
                produto.ImagemUrl,
                produto.CategoriaId,
                categoria.Nome
            );

            return produtoDto.ToSuccessResponse("Produto criado com sucesso");
        }
    }
}
