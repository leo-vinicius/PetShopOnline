using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PetShopAPI.Models.MongoDB
{
    public class AdministradorMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nome")]
        [Required]
        public string Nome { get; set; } = string.Empty;

        [BsonElement("email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BsonElement("senha")]
        [Required]
        public string Senha { get; set; } = string.Empty;

        [BsonElement("dataCriacao")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [BsonElement("ultimoLogin")]
        public DateTime? UltimoLogin { get; set; }

        [BsonElement("ativo")]
        public bool Ativo { get; set; } = true;
    }

    public class ClienteMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nome")]
        [Required]
        public string Nome { get; set; } = string.Empty;

        [BsonElement("email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [BsonElement("senha")]
        [Required]
        public string Senha { get; set; } = string.Empty;

        [BsonElement("telefone")]
        public string? Telefone { get; set; }

        [BsonElement("dataCadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        [BsonElement("ultimoLogin")]
        public DateTime? UltimoLogin { get; set; }

        [BsonElement("ativo")]
        public bool Ativo { get; set; } = true;

        [BsonElement("enderecos")]
        public List<EnderecoMongo> Enderecos { get; set; } = new();
    }

    public class EnderecoMongo
    {
        [BsonElement("_id_endereco")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEndereco { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("logradouro")]
        [Required]
        public string Logradouro { get; set; } = string.Empty;

        [BsonElement("numero")]
        [Required]
        public string Numero { get; set; } = string.Empty;

        [BsonElement("complemento")]
        public string? Complemento { get; set; }

        [BsonElement("cidade")]
        [Required]
        public string Cidade { get; set; } = string.Empty;

        [BsonElement("estado")]
        [Required]
        public string Estado { get; set; } = string.Empty;

        [BsonElement("cep")]
        [Required]
        public string Cep { get; set; } = string.Empty;
    }

    public class SessionMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("userType")]
        public string UserType { get; set; } = string.Empty; // "cliente" ou "administrador"

        [BsonElement("token")]
        public string Token { get; set; } = string.Empty;

        [BsonElement("dataExpiracao")]
        public DateTime DataExpiracao { get; set; }

        [BsonElement("dataCriacao")]
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

        [BsonElement("ativo")]
        public bool Ativo { get; set; } = true;
    }
}
