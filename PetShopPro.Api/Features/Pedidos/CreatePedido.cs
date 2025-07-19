using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using PetShopPro.Api.Infrastructure.Documents;
using PetShopPro.Api.Shared.DTOs;
using System.Security.Claims;

namespace PetShopPro.Api.Features.Pedidos;

public static class CreatePedido
{
    public record Command(
        int EnderecoId,
        ClaimsPrincipal User
    ) : IRequest<ApiResponse<PedidoDto>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.EnderecoId)
                .GreaterThan(0).WithMessage("Endereço é obrigatório");
        }
    }

    public class Handler : IRequestHandler<Command, ApiResponse<PedidoDto>>
    {
        private readonly PetShopContext _context;
        private readonly IMongoContext _mongoContext;
        private readonly Features.Carrinho.ICarrinhoService _carrinhoService;

        public Handler(PetShopContext context, IMongoContext mongoContext, Features.Carrinho.ICarrinhoService carrinhoService)
        {
            _context = context;
            _mongoContext = mongoContext;
            _carrinhoService = carrinhoService;
        }

        public async Task<ApiResponse<PedidoDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userIdClaim = request.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userType = request.User.FindFirst("UserType")?.Value;
            var nomeCliente = request.User.FindFirst(ClaimTypes.Name)?.Value ?? "";

            if (userType != "Cliente" || !int.TryParse(userIdClaim, out var clienteId))
                return ApiResponseExtensions.ToErrorResponse<PedidoDto>("Acesso negado");

            // Verificar se o endereço pertence ao cliente
            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == request.EnderecoId && e.ClienteId == clienteId, cancellationToken);

            if (endereco == null)
                return ApiResponseExtensions.ToErrorResponse<PedidoDto>("Endereço não encontrado");

            // Obter carrinho
            var carrinho = await _carrinhoService.GetCarrinho(clienteId);

            if (!carrinho.Items.Any())
                return ApiResponseExtensions.ToErrorResponse<PedidoDto>("Carrinho está vazio");

            // Verificar e atualizar estoque
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var produtoIds = carrinho.Items.Select(i => i.ProdutoId).ToList();
                var produtos = await _context.Produtos
                    .Where(p => produtoIds.Contains(p.Id))
                    .ToListAsync(cancellationToken);

                foreach (var item in carrinho.Items)
                {
                    var produto = produtos.FirstOrDefault(p => p.Id == item.ProdutoId);
                    if (produto == null || produto.Estoque < item.Quantidade)
                        return ApiResponseExtensions.ToErrorResponse<PedidoDto>($"Produto '{item.ProdutoNome}' não tem estoque suficiente");

                    // Reduzir estoque
                    produto.Estoque -= item.Quantidade;
                }

                await _context.SaveChangesAsync(cancellationToken);

                // Criar pedido no MongoDB
                var enderecoCompleto = endereco.ToEnderecoCompleto();

                var itemsPedido = carrinho.Items.Select((item, index) => new ItemPedido
                {
                    Id = index + 1,
                    ProdutoId = item.ProdutoId,
                    Produto = item.ProdutoNome,
                    Quantidade = item.Quantidade,
                    Preco = item.Preco,
                    Subtotal = item.Subtotal
                }).ToList();

                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                    Nome = nomeCliente,
                    EnderecoId = endereco.Id,
                    EnderecoEntrega = enderecoCompleto,
                    Total = carrinho.Total,
                    DataPedido = DateTime.Now,
                    Status = "Confirmado",
                    Items = itemsPedido
                };

                await _mongoContext.Pedidos.InsertOneAsync(pedido, cancellationToken: cancellationToken);

                // Limpar carrinho
                await _carrinhoService.LimparCarrinho(clienteId);

                await transaction.CommitAsync(cancellationToken);

                var pedidoDto = new PedidoDto(
                    pedido.Id!,
                    pedido.ClienteId,
                    pedido.Nome,
                    pedido.EnderecoId,
                    pedido.EnderecoEntrega,
                    pedido.Total,
                    pedido.DataPedido,
                    pedido.Status,
                    pedido.Items.Select(i => new ItemPedidoDto(i.Id, i.ProdutoId, i.Produto, i.Quantidade, i.Preco, i.Subtotal)).ToList()
                );

                return pedidoDto.ToSuccessResponse("Pedido criado com sucesso! Pagamento aprovado automaticamente.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ApiResponseExtensions.ToErrorResponse<PedidoDto>("Erro ao processar pedido");
            }
        }
    }
}
