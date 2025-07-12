using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopAPI.Models
{
    /// <summary>
    /// Tabela de relacionamento many-to-many entre Produto e Categoria
    /// Implementa RN01: Um produto pode pertencer a uma ou mais categorias
    /// </summary>
    public class ProdutoCategoria
    {
        [Key]
        public int IdProdutoCategoria { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        // Navigation properties
        [ForeignKey("ProdutoId")]
        public virtual Produto Produto { get; set; } = null!;

        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; } = null!;
    }
}
