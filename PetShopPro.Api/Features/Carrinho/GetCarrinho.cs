using MediatR;
using PetShopPro.Api.Shared.DTOs;
using System.Security.Claims;

namespace PetShopPro.Api.Features.Carrinho;

public static class GetCarrinho
{
    public record Query(ClaimsPrincipal User) : IRequest<ApiResponse<CarrinhoDto>>;

    public class Handler : IRequestHandler<Query, ApiResponse<CarrinhoDto>>
    {
        private readonly ICarrinhoService _carrinhoService;

        public Handler(ICarrinhoService carrinhoService)
        {
            _carrinhoService = carrinhoService;
        }

        public async Task<ApiResponse<CarrinhoDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userType = request.User.FindFirst("UserType")?.Value;

            if (userType != "Cliente" || !int.TryParse(userIdClaim, out var clienteId))
                return ApiResponseExtensions.ToErrorResponse<CarrinhoDto>("Acesso negado");

            var carrinho = await _carrinhoService.GetCarrinho(clienteId);
            return carrinho.ToSuccessResponse();
        }
    }
}
