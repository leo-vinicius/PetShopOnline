using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Infrastructure.Entities;
using PetShopPro.Api.Shared.DTOs;
using PetShopPro.Api.Shared.Services;

namespace PetShopPro.Api.Features.Clientes;

public static class CreateCliente
{
    public record Command(
        string Nome,
        string Email,
        string Telefone,
        string Senha
    ) : IRequest<ApiResponse<ClienteDto>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter um formato válido")
                .MaximumLength(200).WithMessage("Email deve ter no máximo 200 caracteres");

            RuleFor(x => x.Telefone)
                .NotEmpty().WithMessage("Telefone é obrigatório")
                .MaximumLength(20).WithMessage("Telefone deve ter no máximo 20 caracteres");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres");
        }
    }

    public class Handler : IRequestHandler<Command, ApiResponse<ClienteDto>>
    {
        private readonly PetShopContext _context;
        private readonly IPasswordService _passwordService;

        public Handler(PetShopContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<ApiResponse<ClienteDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            // Verifica se já existe cliente com este email
            var emailExists = await _context.Clientes
                .AnyAsync(c => c.Email == request.Email, cancellationToken);

            if (emailExists)
                return ApiResponseExtensions.ToErrorResponse<ClienteDto>("Já existe um cliente com este email");

            var cliente = new Cliente
            {
                Nome = request.Nome,
                Email = request.Email,
                Telefone = request.Telefone,
                Senha = _passwordService.HashPassword(request.Senha),
                Ativo = true
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync(cancellationToken);

            var clienteDto = new ClienteDto(
                cliente.Id,
                cliente.Nome,
                cliente.Email,
                cliente.Telefone,
                cliente.Ativo
            );

            return clienteDto.ToSuccessResponse("Cliente criado com sucesso");
        }
    }
}
