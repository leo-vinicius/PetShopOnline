namespace PetShopPro.Api.Features.Pedidos;

public record PedidoDto(
    string Id,
    int ClienteId,
    string Nome,
    int EnderecoId,
    string EnderecoEntrega,
    decimal Total,
    DateTime DataPedido,
    string Status,
    List<ItemPedidoDto> Items
);

public record ItemPedidoDto(
    int Id,
    int ProdutoId,
    string Produto,
    int Quantidade,
    decimal Preco,
    decimal Subtotal
);

public record CreatePedidoRequest(
    int EnderecoId
);

public record EnderecoEntregaDto(
    int Id,
    string EnderecoCompleto
);
