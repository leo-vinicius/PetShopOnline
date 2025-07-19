using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Infrastructure.Entities;
using PetShopPro.Api.Shared.DTOs;
using System.Security.Claims;

namespace PetShopPro.Api.Features.Clientes;

public static class CreateEndereco
{
    public record Command(
        string Logradouro,
        int? Numero,
        string? Bairro,
        string Cidade,
        string Estado,
        string CEP,
        ClaimsPrincipal User
    ) : IRequest<ApiResponse<EnderecoDto>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Logradouro)
                .NotEmpty().WithMessage("Logradouro é obrigatório")
                .MaximumLength(300).WithMessage("Logradouro deve ter no máximo 300 caracteres");

            RuleFor(x => x.Cidade)
                .NotEmpty().WithMessage("Cidade é obrigatória")
                .MaximumLength(100).WithMessage("Cidade deve ter no máximo 100 caracteres");

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("Estado é obrigatório")
                .Length(2).WithMessage("Estado deve ter 2 caracteres");

            RuleFor(x => x.CEP)
                .NotEmpty().WithMessage("CEP é obrigatório")
                .Matches(@"^\d{5}-?\d{3}$").WithMessage("CEP deve ter o formato 12345-678 ou 12345678");

            RuleFor(x => x.Bairro)
                .MaximumLength(100).WithMessage("Bairro deve ter no máximo 100 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Bairro));
        }
    }

    public class Handler : IRequestHandler<Command, ApiResponse<EnderecoDto>>
    {
        private readonly PetShopContext _context;

        public Handler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<EnderecoDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userType = request.User.FindFirst("UserType")?.Value;

            if (userType != "Cliente" || !int.TryParse(userIdClaim, out var clienteId))
                return ApiResponseExtensions.ToErrorResponse<EnderecoDto>("Acesso negado");

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == clienteId && c.Ativo, cancellationToken);

            if (cliente == null)
                return ApiResponseExtensions.ToErrorResponse<EnderecoDto>("Cliente não encontrado");

            var endereco = new Endereco
            {
                Logradouro = request.Logradouro,
                Numero = request.Numero,
                Bairro = request.Bairro,
                Cidade = request.Cidade,
                Estado = request.Estado.ToUpper(),
                CEP = request.CEP.Replace("-", ""),
                ClienteId = clienteId
            };

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync(cancellationToken);

            var enderecoDto = new EnderecoDto(
                endereco.Id,
                endereco.Logradouro,
                endereco.Numero,
                endereco.Bairro,
                endereco.Cidade,
                endereco.Estado,
                endereco.CEP
            );

            return enderecoDto.ToSuccessResponse("Endereço criado com sucesso");
        }
    }
}
