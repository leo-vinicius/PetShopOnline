using System.ComponentModel.DataAnnotations;
using PetShopAPI.Models;

namespace PetShopAPI.DTOs
{
    public class PedidoDto
    {
        public int IdPedido { get; set; }
        public DateTime DataPedido { get; set; }
        public StatusPedido Status { get; set; }
        public decimal ValorTotal { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;
        public int EnderecoEntregaId { get; set; }
        public EnderecoDto EnderecoEntrega { get; set; } = new();
        public List<ItemPedidoDto> Itens { get; set; } = new();
    }

    public class PedidoCreateDto
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int EnderecoEntregaId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Pedido deve conter pelo menos um item")]
        public List<ItemPedidoCreateDto> Itens { get; set; } = new();
    }

    public class ItemPedidoDto
    {
        public int IdItem { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
    }

    public class ItemPedidoCreateDto
    {
        [Required]
        public int ProdutoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }
    }

    public class CarrinhoDto
    {
        public List<ItemCarrinhoDto> Itens { get; set; } = new();
        public decimal ValorTotal => Itens.Sum(i => i.Subtotal);
    }

    public class ItemCarrinhoDto
    {
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal => PrecoUnitario * Quantidade;
        public string? ImagemUrl { get; set; }
    }

    public class AdicionarCarrinhoDto
    {
        [Required]
        public int ProdutoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantidade deve ser maior que zero")]
        public int Quantidade { get; set; }
    }
}
