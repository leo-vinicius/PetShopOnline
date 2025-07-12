using System.ComponentModel.DataAnnotations;

namespace PetShopAPI.DTOs
{
    public class ProdutoDto
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public bool Ativo { get; set; }
        public string? ImagemUrl { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;

        // Categorias adicionais para implementar RN01 (opcional)
        public List<CategoriaDto> CategoriasAdicionais { get; set; } = new();
    }

    public class ProdutoCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Estoque deve ser maior ou igual a zero")]
        public int Estoque { get; set; }

        public bool Ativo { get; set; } = true;

        [StringLength(255)]
        public string? ImagemUrl { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        // Categorias adicionais para RN01 (opcional)
        public List<int> CategoriaIdsAdicionais { get; set; } = new();
    }

    public class ProdutoUpdateDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Estoque deve ser maior ou igual a zero")]
        public int Estoque { get; set; }

        public bool Ativo { get; set; }

        [StringLength(255)]
        public string? ImagemUrl { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        // Categorias adicionais para RN01 (opcional)
        public List<int> CategoriaIdsAdicionais { get; set; } = new();
    }

    public class CategoriaDto
    {
        public int IdCategoria { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

    public class CategoriaCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;
    }
}
