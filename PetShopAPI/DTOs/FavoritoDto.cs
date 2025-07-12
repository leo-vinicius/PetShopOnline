using System.ComponentModel.DataAnnotations;

namespace PetShopAPI.DTOs
{
    /// <summary>
    /// DTO para operações com favoritos
    /// Implementa RF12: Sistema deverá permitir que o cliente possa favoritar produtos
    /// </summary>
    public class FavoritoDto
    {
        public int IdFavorito { get; set; }
        public int ClienteId { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public decimal ProdutoPreco { get; set; }
        public string? ProdutoImagemUrl { get; set; }
        public DateTime DataFavorito { get; set; }
    }

    public class AdicionarFavoritoDto
    {
        [Required]
        public int ProdutoId { get; set; }
    }

    public class ProdutoComFavoritoDto
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public bool Ativo { get; set; }
        public string? ImagemUrl { get; set; }
        public List<string> Categorias { get; set; } = new();
        public bool IsFavorito { get; set; }
    }
}
