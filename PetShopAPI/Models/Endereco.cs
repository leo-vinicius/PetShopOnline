using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopAPI.Models
{
    public class Endereco
    {
        [Key]
        public int IdEndereco { get; set; }

        [Required]
        [StringLength(150)]
        public string Logradouro { get; set; } = string.Empty;

        [Required]
        public int Numero { get; set; }

        [StringLength(100)]
        public string? Bairro { get; set; }

        [Required]
        [StringLength(100)]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Cep { get; set; } = string.Empty;

        [Required]
        public int ClienteId { get; set; }

        // Navigation property
        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; } = null!;
        
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
