using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Shared.DTOs;
using PetShopPro.Api.Shared.Services;

namespace PetShopPro.Api.Features.Auth;

public static class LoginCliente
{
    public record Command(string Email, string Senha) : IRequest<ApiResponse<LoginResponse>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve ter um formato válido");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória");
        }
    }

    public class Handler : IRequestHandler<Command, ApiResponse<LoginResponse>>
    {
        private readonly PetShopContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public Handler(PetShopContext context, IPasswordService passwordService, IJwtService jwtService)
        {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<ApiResponse<LoginResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Email == request.Email && c.Ativo, cancellationToken);

            if (cliente == null)
                return ApiResponseExtensions.ToErrorResponse<LoginResponse>("Email ou senha inválidos");

            if (!_passwordService.VerifyPassword(request.Senha, cliente.Senha))
                return ApiResponseExtensions.ToErrorResponse<LoginResponse>("Email ou senha inválidos");

            var token = _jwtService.GenerateToken(cliente.Id, cliente.Email, "Cliente", cliente.Nome);

            var response = new LoginResponse(token, "Cliente", cliente.Id, cliente.Nome, cliente.Email);
            return response.ToSuccessResponse("Login realizado com sucesso");
        }
    }
}
