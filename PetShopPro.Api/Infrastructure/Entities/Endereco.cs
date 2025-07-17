using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopPro.Api.Infrastructure.Entities;

public class Endereco
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(300)]
    public string Logradouro { get; set; } = string.Empty;

    public int? Numero { get; set; }

    [MaxLength(100)]
    public string? Bairro { get; set; }

    [Required]
    [MaxLength(100)]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    public string Estado { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string CEP { get; set; } = string.Empty;

    [Required]
    public int ClienteId { get; set; }

    // Navigation property
    [ForeignKey("ClienteId")]
    public virtual Cliente Cliente { get; set; } = null!;
}
