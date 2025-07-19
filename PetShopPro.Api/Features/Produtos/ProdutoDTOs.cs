namespace PetShopPro.Api.Features.Produtos;

public record ProdutoDto(
    int Id,
    string Nome,
    string? Descricao,
    decimal Preco,
    int Estoque,
    bool Ativo,
    string? ImagemUrl,
    int CategoriaId,
    string CategoriaNome
);

public record CreateProdutoRequest(
    string Nome,
    string? Descricao,
    decimal Preco,
    int Estoque,
    string? ImagemUrl,
    int CategoriaId
);

public record UpdateProdutoRequest(
    string Nome,
    string? Descricao,
    decimal Preco,
    int Estoque,
    string? ImagemUrl,
    int CategoriaId
);
