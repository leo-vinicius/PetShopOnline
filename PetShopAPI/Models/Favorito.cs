using System.ComponentModel.DataAnnotations;

namespace PetShopAPI.Models
{
    /// <summary>
    /// Modelo para produtos favoritos dos clientes
    /// Implementa RF12: Sistema deverá permitir que o cliente possa favoritar produtos
    /// </summary>
    public class Favorito
    {
        [Key]
        public int IdFavorito { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        public DateTime DataFavorito { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Cliente Cliente { get; set; } = null!;
        public virtual Produto Produto { get; set; } = null!;
    }
}
