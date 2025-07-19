using System.ComponentModel.DataAnnotations;

namespace PetShopPro.Api.Infrastructure.Entities;

public class Categoria
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;

    // Navigation property
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
