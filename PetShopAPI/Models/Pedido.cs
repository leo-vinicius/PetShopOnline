using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopAPI.Models
{
    public enum StatusPedido
    {
        PENDENTE,
        PROCESSANDO,
        ENVIADO,
        ENTREGUE,
        CANCELADO
    }

    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        [Required]
        public DateTime DataPedido { get; set; } = DateTime.Now;

        [Required]
        public StatusPedido Status { get; set; } = StatusPedido.PENDENTE;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Valor total deve ser maior que zero")]
        public decimal ValorTotal { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int EnderecoEntregaId { get; set; }

        // Navigation properties
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; } = null!;

        [ForeignKey("EnderecoEntregaId")]
        public virtual Endereco EnderecoEntrega { get; set; } = null!;

        public virtual ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
    }
}
