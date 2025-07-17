using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Infrastructure.Entities;
using PetShopPro.Api.Shared.DTOs;
using PetShopPro.Api.Shared.Services;

namespace PetShopPro.Api.Features.Administradores;

public static class CreateAdministrador
{
    public record Command(
        string Nome,
        string Email,
        string Telefone,
        string Senha
    ) : IRequest<ApiResponse<AdministradorDto>>;

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

    public class Handler : IRequestHandler<Command, ApiResponse<AdministradorDto>>
    {
        private readonly PetShopContext _context;
        private readonly IPasswordService _passwordService;

        public Handler(PetShopContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<ApiResponse<AdministradorDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            // Verifica se já existe administrador com este email
            var emailExists = await _context.Administradores
                .AnyAsync(a => a.Email == request.Email, cancellationToken);

            if (emailExists)
                return ApiResponseExtensions.ToErrorResponse<AdministradorDto>("Já existe um administrador com este email");

            var administrador = new Administrador
            {
                Nome = request.Nome,
                Email = request.Email,
                Telefone = request.Telefone,
                Senha = _passwordService.HashPassword(request.Senha),
                Ativo = true
            };

            _context.Administradores.Add(administrador);
            await _context.SaveChangesAsync(cancellationToken);

            var administradorDto = new AdministradorDto(
                administrador.Id,
                administrador.Nome,
                administrador.Email,
                administrador.Telefone,
                administrador.Ativo
            );

            return administradorDto.ToSuccessResponse("Administrador criado com sucesso");
        }
    }
}
