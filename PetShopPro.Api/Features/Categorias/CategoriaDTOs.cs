namespace PetShopPro.Api.Features.Categorias;

public record CategoriaDto(
    int Id,
    string Nome,
    bool Ativo
);

public record CreateCategoriaRequest(
    string Nome
);

public record UpdateCategoriaRequest(
    string Nome
);
