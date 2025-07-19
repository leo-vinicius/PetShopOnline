using System.ComponentModel.DataAnnotations;

namespace PetShopPro.Api.Infrastructure.Entities;

public class Cliente
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Telefone { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Senha { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;

    // Navigation property
    public virtual ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
}
