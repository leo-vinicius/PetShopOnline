using FluentValidation;
using MediatR;
using PetShopPro.Api.Shared.DTOs;
using System.Security.Claims;

namespace PetShopPro.Api.Features.Carrinho;

public static class AdicionarItemCarrinho
{
    public record Command(
        int ProdutoId,
        int Quantidade,
        ClaimsPrincipal User
    ) : IRequest<ApiResponse<CarrinhoDto>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ProdutoId)
                .GreaterThan(0).WithMessage("Produto é obrigatório");

            RuleFor(x => x.Quantidade)
                .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero");
        }
    }

    public class Handler : IRequestHandler<Command, ApiResponse<CarrinhoDto>>
    {
        private readonly ICarrinhoService _carrinhoService;

        public Handler(ICarrinhoService carrinhoService)
        {
            _carrinhoService = carrinhoService;
        }

        public async Task<ApiResponse<CarrinhoDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userType = request.User.FindFirst("UserType")?.Value;

            if (userType != "Cliente" || !int.TryParse(userIdClaim, out var clienteId))
                return ApiResponseExtensions.ToErrorResponse<CarrinhoDto>("Acesso negado");

            var sucesso = await _carrinhoService.AdicionarItem(clienteId, request.ProdutoId, request.Quantidade);

            if (!sucesso)
                return ApiResponseExtensions.ToErrorResponse<CarrinhoDto>("Não foi possível adicionar o item ao carrinho. Verifique se o produto existe e se há estoque suficiente");

            var carrinho = await _carrinhoService.GetCarrinho(clienteId);
            return carrinho.ToSuccessResponse("Item adicionado ao carrinho com sucesso");
        }
    }
}
