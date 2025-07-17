using MediatR;
using MongoDB.Driver;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Shared.DTOs;
using System.Security.Claims;

namespace PetShopPro.Api.Features.Pedidos;

public static class GetMeusPedidos
{
    public record Query(ClaimsPrincipal User) : IRequest<ApiResponse<List<PedidoDto>>>;

    public class Handler : IRequestHandler<Query, ApiResponse<List<PedidoDto>>>
    {
        private readonly IMongoContext _mongoContext;

        public Handler(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public async Task<ApiResponse<List<PedidoDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userType = request.User.FindFirst("UserType")?.Value;

            if (userType != "Cliente" || !int.TryParse(userIdClaim, out var clienteId))
                return ApiResponseExtensions.ToErrorResponse<List<PedidoDto>>("Acesso negado");

            var pedidos = await _mongoContext.Pedidos
                .Find(p => p.ClienteId == clienteId)
                .SortByDescending(p => p.DataPedido)
                .ToListAsync(cancellationToken);

            var pedidosDto = pedidos.Select(p => new PedidoDto(
                p.Id!,
                p.ClienteId,
                p.Nome,
                p.EnderecoId,
                p.EnderecoEntrega,
                p.Total,
                p.DataPedido,
                p.Status,
                p.Items.Select(i => new ItemPedidoDto(i.Id, i.ProdutoId, i.Produto, i.Quantidade, i.Preco, i.Subtotal)).ToList()
            )).ToList();

            return pedidosDto.ToSuccessResponse();
        }
    }
}
