using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopAPI.Models
{
    public class ItemPedido
    {
        [Key]
        public int IdItem { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Preço unitário deve ser maior que zero")]
        public decimal PrecoUnitario { get; set; }

        [Required]
        public int ProdutoId { get; set; }

        [Required]
        public int PedidoId { get; set; }

        // Navigation properties
        [ForeignKey("ProdutoId")]
        public virtual Produto Produto { get; set; } = null!;

        [ForeignKey("PedidoId")]
        public virtual Pedido Pedido { get; set; } = null!;

        // Calculated property
        [NotMapped]
        public decimal Subtotal => Quantidade * PrecoUnitario;
    }
}
