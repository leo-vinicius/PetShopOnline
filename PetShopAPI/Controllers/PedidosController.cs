using Microsoft.AspNetCore.Mvc;
using PetShopAPI.DTOs;
using PetShopAPI.Models;
using PetShopAPI.Repositories;
using PetShopAPI.Services;

namespace PetShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly ICarrinhoService _carrinhoService;

        public PedidosController(
            IPedidoRepository pedidoRepository,
            IClienteRepository clienteRepository,
            IProdutoRepository produtoRepository,
            IEnderecoRepository enderecoRepository,
            ICarrinhoService carrinhoService)
        {
            _pedidoRepository = pedidoRepository;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
            _enderecoRepository = enderecoRepository;
            _carrinhoService = carrinhoService;
        }

        /// <summary>
        /// Obtém todos os pedidos (Admin)
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> GetPedidos()
        {
            var pedidos = await _pedidoRepository.GetAllAsync();

            var pedidosDto = pedidos.Select(p => MapToPedidoDto(p));

            return Ok(pedidosDto);
        }

        /// <summary>
        /// Obtém pedidos de um cliente específico
        /// </summary>
        [HttpGet("cliente/{clienteId}")]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> GetPedidosByCliente(int clienteId)
        {
            if (!await _clienteRepository.ExistsAsync(clienteId))
                return NotFound($"Cliente com ID {clienteId} não encontrado.");

            var pedidos = await _pedidoRepository.GetByClienteIdAsync(clienteId);

            var pedidosDto = pedidos.Select(p => MapToPedidoDto(p));

            return Ok(pedidosDto);
        }

        /// <summary>
        /// Obtém um pedido específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDto>> GetPedido(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);

            if (pedido == null)
                return NotFound($"Pedido com ID {id} não encontrado.");

            return Ok(MapToPedidoDto(pedido));
        }

        /// <summary>
        /// Cria um novo pedido
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<PedidoDto>> CreatePedido(PedidoCreateDto pedidoCreateDto)
        {
            // Validações
            if (!await _clienteRepository.ExistsAsync(pedidoCreateDto.ClienteId))
                return BadRequest($"Cliente com ID {pedidoCreateDto.ClienteId} não encontrado.");

            if (!await _enderecoRepository.ExistsAsync(pedidoCreateDto.EnderecoEntregaId))
                return BadRequest($"Endereço com ID {pedidoCreateDto.EnderecoEntregaId} não encontrado.");

            var itens = new List<ItemPedido>();
            decimal valorTotal = 0;

            // Validar produtos e calcular valor total
            foreach (var itemDto in pedidoCreateDto.Itens)
            {
                var produto = await _produtoRepository.GetByIdAsync(itemDto.ProdutoId);
                if (produto == null)
                    return BadRequest($"Produto com ID {itemDto.ProdutoId} não encontrado.");

                if (!produto.Ativo)
                    return BadRequest($"Produto '{produto.Nome}' não está disponível.");

                if (produto.Estoque < itemDto.Quantidade)
                    return BadRequest($"Estoque insuficiente para o produto '{produto.Nome}'. Disponível: {produto.Estoque}");

                var item = new ItemPedido
                {
                    ProdutoId = itemDto.ProdutoId,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.Preco
                };

                itens.Add(item);
                valorTotal += item.Subtotal;

                // Atualizar estoque
                produto.Estoque -= itemDto.Quantidade;
                await _produtoRepository.UpdateAsync(produto);
            }

            var pedido = new Pedido
            {
                ClienteId = pedidoCreateDto.ClienteId,
                EnderecoEntregaId = pedidoCreateDto.EnderecoEntregaId,
                DataPedido = DateTime.Now,
                Status = StatusPedido.PENDENTE,
                ValorTotal = valorTotal,
                Itens = itens
            };

            await _pedidoRepository.CreateAsync(pedido);

            // RN08: Esvaziar carrinho automaticamente após finalização do pedido
            _carrinhoService.LimparCarrinho(pedidoCreateDto.ClienteId);

            // Recarregar com navegação para retorno
            pedido = await _pedidoRepository.GetByIdAsync(pedido.IdPedido);

            return CreatedAtAction(nameof(GetPedido), new { id = pedido!.IdPedido }, MapToPedidoDto(pedido));
        }

        /// <summary>
        /// Atualiza o status de um pedido (Admin)
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusPedido novoStatus)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);

            if (pedido == null)
                return NotFound($"Pedido com ID {id} não encontrado.");

            pedido.Status = novoStatus;
            await _pedidoRepository.UpdateAsync(pedido);

            return NoContent();
        }

        /// <summary>
        /// Cancela um pedido
        /// </summary>
        [HttpPatch("{id}/cancelar")]
        public async Task<IActionResult> CancelarPedido(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);

            if (pedido == null)
                return NotFound($"Pedido com ID {id} não encontrado.");

            if (pedido.Status == StatusPedido.ENTREGUE)
                return BadRequest("Não é possível cancelar um pedido já entregue.");

            if (pedido.Status == StatusPedido.CANCELADO)
                return BadRequest("Pedido já está cancelado.");

            // Devolver itens ao estoque
            foreach (var item in pedido.Itens)
            {
                var produto = await _produtoRepository.GetByIdAsync(item.ProdutoId);
                if (produto != null)
                {
                    produto.Estoque += item.Quantidade;
                    await _produtoRepository.UpdateAsync(produto);
                }
            }

            pedido.Status = StatusPedido.CANCELADO;
            await _pedidoRepository.UpdateAsync(pedido);

            return NoContent();
        }

        private PedidoDto MapToPedidoDto(Pedido pedido)
        {
            return new PedidoDto
            {
                IdPedido = pedido.IdPedido,
                DataPedido = pedido.DataPedido,
                Status = pedido.Status,
                ValorTotal = pedido.ValorTotal,
                ClienteId = pedido.ClienteId,
                ClienteNome = pedido.Cliente.Nome,
                EnderecoEntregaId = pedido.EnderecoEntregaId,
                EnderecoEntrega = new EnderecoDto
                {
                    IdEndereco = pedido.EnderecoEntrega.IdEndereco,
                    Logradouro = pedido.EnderecoEntrega.Logradouro,
                    Numero = pedido.EnderecoEntrega.Numero,
                    Bairro = pedido.EnderecoEntrega.Bairro,
                    Cidade = pedido.EnderecoEntrega.Cidade,
                    Estado = pedido.EnderecoEntrega.Estado,
                    Cep = pedido.EnderecoEntrega.Cep,
                    ClienteId = pedido.EnderecoEntrega.ClienteId
                },
                Itens = pedido.Itens.Select(i => new ItemPedidoDto
                {
                    IdItem = i.IdItem,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    Subtotal = i.Subtotal,
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.Produto.Nome
                }).ToList()
            };
        }
    }
}
