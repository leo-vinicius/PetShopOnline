using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopAPI.Models
{
    public class Produto
    {
        [Key]
        public int IdProduto { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Estoque deve ser maior ou igual a zero")]
        public int Estoque { get; set; }

        public bool Ativo { get; set; } = true;

        [StringLength(255)]
        public string? ImagemUrl { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        // Navigation properties
        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; } = null!;

        // Many-to-many relationship for additional categories (RN01)
        public virtual ICollection<ProdutoCategoria> ProdutoCategorias { get; set; } = new List<ProdutoCategoria>();
        public virtual ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
        public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();
    }
}
