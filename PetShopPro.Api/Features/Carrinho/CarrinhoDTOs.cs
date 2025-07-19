namespace PetShopPro.Api.Features.Carrinho;

public record ItemCarrinhoDto(
    int ProdutoId,
    string ProdutoNome,
    decimal Preco,
    int Quantidade,
    decimal Subtotal,
    int EstoqueDisponivel
);

public record CarrinhoDto(
    List<ItemCarrinhoDto> Items,
    decimal Total,
    int TotalItems
);

public record AdicionarItemRequest(
    int ProdutoId,
    int Quantidade
);

public record AtualizarQuantidadeRequest(
    int ProdutoId,
    int Quantidade
);
