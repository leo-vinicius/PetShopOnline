using System.ComponentModel.DataAnnotations;

namespace PetShopAPI.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }

    public class ClienteRegistroDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Senha { get; set; } = string.Empty;

        [Phone]
        public string? Telefone { get; set; }
    }

    public class ClienteDto
    {
        public int IdCliente { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public List<EnderecoDto> Enderecos { get; set; } = new();
    }

    public class EnderecoDto
    {
        public int IdEndereco { get; set; }
        public string Logradouro { get; set; } = string.Empty;
        public int Numero { get; set; }
        public string? Bairro { get; set; }
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public int ClienteId { get; set; }
    }

    public class EnderecoCreateDto
    {
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
    }

    public class AdministradorRegistroDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Senha { get; set; } = string.Empty;
    }
}
