using PetShopPro.Api.Infrastructure.Entities;

namespace PetShopPro.Api.Features.Pedidos;

public static class EnderecoExtensions
{
    public static string ToEnderecoCompleto(this Endereco endereco)
    {
        return $"{endereco.Logradouro}" +
               (endereco.Numero.HasValue ? $", {endereco.Numero}" : "") +
               (!string.IsNullOrEmpty(endereco.Bairro) ? $", {endereco.Bairro}" : "") +
               $", {endereco.Cidade} - {endereco.Estado}, CEP: {endereco.CEP}";
    }

    public static EnderecoEntregaDto ToEnderecoEntregaDto(this Endereco endereco)
    {
        return new EnderecoEntregaDto(endereco.Id, endereco.ToEnderecoCompleto());
    }
}
