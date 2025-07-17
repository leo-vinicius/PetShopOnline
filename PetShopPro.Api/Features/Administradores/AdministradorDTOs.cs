namespace PetShopPro.Api.Features.Administradores;

public record AdministradorDto(
    int Id,
    string Nome,
    string Email,
    string Telefone,
    bool Ativo
);

public record CreateAdministradorRequest(
    string Nome,
    string Email,
    string Telefone,
    string Senha
);
