using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShopPro.Api.Infrastructure.Entities;

public class Produto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    [Required]
    public int Estoque { get; set; }

    public bool Ativo { get; set; } = true;

    [MaxLength(2000)]
    public string? ImagemUrl { get; set; }

    [Required]
    public int CategoriaId { get; set; }

    // Navigation property
    [ForeignKey("CategoriaId")]
    public virtual Categoria Categoria { get; set; } = null!;
}
