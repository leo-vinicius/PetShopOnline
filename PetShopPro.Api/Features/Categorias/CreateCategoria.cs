using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Infrastructure.Entities;
using PetShopPro.Api.Shared.DTOs;

namespace PetShopPro.Api.Features.Categorias;

public static class CreateCategoria
{
    public record Command(string Nome) : IRequest<ApiResponse<CategoriaDto>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");
        }
    }

    public class Handler : IRequestHandler<Command, ApiResponse<CategoriaDto>>
    {
        private readonly PetShopContext _context;

        public Handler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<CategoriaDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            // Verifica se já existe categoria com este nome
            var nomeExists = await _context.Categorias
                .AnyAsync(c => c.Nome.ToLower() == request.Nome.ToLower() && c.Ativo, cancellationToken);

            if (nomeExists)
                return ApiResponseExtensions.ToErrorResponse<CategoriaDto>("Já existe uma categoria com este nome");

            var categoria = new Categoria
            {
                Nome = request.Nome,
                Ativo = true
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync(cancellationToken);

            var categoriaDto = new CategoriaDto(
                categoria.Id,
                categoria.Nome,
                categoria.Ativo
            );

            return categoriaDto.ToSuccessResponse("Categoria criada com sucesso");
        }
    }
}
