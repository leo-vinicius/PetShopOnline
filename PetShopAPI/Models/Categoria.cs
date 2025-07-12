using System.ComponentModel.DataAnnotations;

namespace PetShopAPI.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();

        // Many-to-many relationship for additional product-category associations (RN01)
        public virtual ICollection<ProdutoCategoria> ProdutoCategorias { get; set; } = new List<ProdutoCategoria>();
    }
}
