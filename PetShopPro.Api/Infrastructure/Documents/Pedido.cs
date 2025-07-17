using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PetShopPro.Api.Infrastructure.Documents;

public class Pedido
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("clienteId")]
    public int ClienteId { get; set; }

    [BsonElement("nome")]
    public string Nome { get; set; } = string.Empty;

    [BsonElement("enderecoId")]
    public int EnderecoId { get; set; }

    [BsonElement("enderecoEntrega")]
    public string EnderecoEntrega { get; set; } = string.Empty;

    [BsonElement("total")]
    public decimal Total { get; set; }

    [BsonElement("dataPedido")]
    public DateTime DataPedido { get; set; } = DateTime.Now;

    [BsonElement("status")]
    public string Status { get; set; } = "Confirmado";

    [BsonElement("items")]
    public List<ItemPedido> Items { get; set; } = new();
}

public class ItemPedido
{
    [BsonElement("id")]
    public int Id { get; set; }

    [BsonElement("produtoId")]
    public int ProdutoId { get; set; }

    [BsonElement("produto")]
    public string Produto { get; set; } = string.Empty;

    [BsonElement("quantidade")]
    public int Quantidade { get; set; }

    [BsonElement("preco")]
    public decimal Preco { get; set; }

    [BsonElement("subtotal")]
    public decimal Subtotal { get; set; }
}
