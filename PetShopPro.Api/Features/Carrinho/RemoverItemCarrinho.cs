using FluentValidation;
using MediatR;
using PetShopPro.Api.Shared.DTOs;
using System.Security.Claims;

namespace PetShopPro.Api.Features.Carrinho;

public static class RemoverItemCarrinho
{
    public record Command(
        int ProdutoId,
        ClaimsPrincipal User
    ) : IRequest<ApiResponse<CarrinhoDto>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.ProdutoId)
                .GreaterThan(0).WithMessage("Produto é obrigatório");
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

            var sucesso = await _carrinhoService.RemoverItem(clienteId, request.ProdutoId);

            if (!sucesso)
                return ApiResponseExtensions.ToErrorResponse<CarrinhoDto>("Não foi possível remover o item do carrinho. Verifique se o produto existe no carrinho");

            var carrinho = await _carrinhoService.GetCarrinho(clienteId);
            return carrinho.ToSuccessResponse("Item removido do carrinho com sucesso");
        }
    }
}
