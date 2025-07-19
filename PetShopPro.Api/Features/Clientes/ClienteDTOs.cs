namespace PetShopPro.Api.Features.Clientes;

public record ClienteDto(
    int Id,
    string Nome,
    string Email,
    string Telefone,
    bool Ativo
);

public record CreateClienteRequest(
    string Nome,
    string Email,
    string Telefone,
    string Senha
);

public record UpdateClienteRequest(
    string Nome,
    string Email,
    string Telefone
);

public record EnderecoDto(
    int Id,
    string Logradouro,
    int? Numero,
    string? Bairro,
    string Cidade,
    string Estado,
    string CEP
);

public record CreateEnderecoRequest(
    string Logradouro,
    int? Numero,
    string? Bairro,
    string Cidade,
    string Estado,
    string CEP
);
