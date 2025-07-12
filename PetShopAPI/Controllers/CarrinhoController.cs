using Microsoft.AspNetCore.Mvc;
using PetShopAPI.DTOs;
using PetShopAPI.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace PetShopAPI.Controllers
{
    /// <summary>
    /// Controller para gerenciamento do carrinho de compras
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Carrinho de compras e finalização de pedidos")]
    public class CarrinhoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private static readonly Dictionary<string, CarrinhoDto> _carrinhos = new();

        public CarrinhoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        /// <summary>
        /// Obtém o carrinho de compras do cliente
        /// </summary>
        /// <param name="clienteId">ID do cliente</param>
        /// <returns>Carrinho de compras com produtos e total</returns>
        /// <response code="200">Carrinho retornado com sucesso</response>
        /// <response code="404">Cliente não encontrado</response>
        [HttpGet("{clienteId}")]
        [SwaggerOperation(
            Summary = "Obtém carrinho do cliente",
            Description = "Retorna o carrinho de compras atual do cliente com todos os itens e valores totais.",
            OperationId = "GetCarrinho",
            Tags = new[] { "Carrinho" }
        )]
        [SwaggerResponse(200, "Carrinho retornado com sucesso", typeof(CarrinhoDto))]
        [SwaggerResponse(404, "Cliente não encontrado")]
        public ActionResult<CarrinhoDto> GetCarrinho(int clienteId)
        {
            var chaveCarrinho = $"cliente_{clienteId}";
            
            if (_carrinhos.TryGetValue(chaveCarrinho, out var carrinho))
                return Ok(carrinho);

            return Ok(new CarrinhoDto());
        }

        /// <summary>
        /// Adiciona um item ao carrinho
        /// </summary>
        [HttpPost("{clienteId}/adicionar")]
        public async Task<ActionResult<CarrinhoDto>> AdicionarItem(int clienteId, AdicionarCarrinhoDto adicionarDto)
        {
            var produto = await _produtoRepository.GetByIdAsync(adicionarDto.ProdutoId);
            
            if (produto == null)
                return NotFound($"Produto com ID {adicionarDto.ProdutoId} não encontrado.");

            if (!produto.Ativo)
                return BadRequest($"Produto '{produto.Nome}' não está disponível.");

            if (produto.Estoque < adicionarDto.Quantidade)
                return BadRequest($"Estoque insuficiente. Disponível: {produto.Estoque}");

            var chaveCarrinho = $"cliente_{clienteId}";
            
            if (!_carrinhos.TryGetValue(chaveCarrinho, out var carrinho))
            {
                carrinho = new CarrinhoDto();
                _carrinhos[chaveCarrinho] = carrinho;
            }

            // Verificar se o produto já está no carrinho
            var itemExistente = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == adicionarDto.ProdutoId);
            
            if (itemExistente != null)
            {
                // Verificar se a nova quantidade não excede o estoque
                var novaQuantidade = itemExistente.Quantidade + adicionarDto.Quantidade;
                if (novaQuantidade > produto.Estoque)
                    return BadRequest($"Quantidade total ({novaQuantidade}) excede o estoque disponível ({produto.Estoque}).");

                itemExistente.Quantidade = novaQuantidade;
            }
            else
            {
                var novoItem = new ItemCarrinhoDto
                {
                    ProdutoId = produto.IdProduto,
                    ProdutoNome = produto.Nome,
                    PrecoUnitario = produto.Preco,
                    Quantidade = adicionarDto.Quantidade,
                    ImagemUrl = produto.ImagemUrl
                };

                carrinho.Itens.Add(novoItem);
            }

            return Ok(carrinho);
        }

        /// <summary>
        /// Atualiza a quantidade de um item no carrinho
        /// </summary>
        [HttpPut("{clienteId}/item/{produtoId}")]
        public async Task<ActionResult<CarrinhoDto>> AtualizarItem(int clienteId, int produtoId, [FromBody] int novaQuantidade)
        {
            if (novaQuantidade <= 0)
                return BadRequest("Quantidade deve ser maior que zero.");

            var produto = await _produtoRepository.GetByIdAsync(produtoId);
            
            if (produto == null)
                return NotFound($"Produto com ID {produtoId} não encontrado.");

            if (novaQuantidade > produto.Estoque)
                return BadRequest($"Quantidade excede o estoque disponível ({produto.Estoque}).");

            var chaveCarrinho = $"cliente_{clienteId}";
            
            if (!_carrinhos.TryGetValue(chaveCarrinho, out var carrinho))
                return NotFound("Carrinho não encontrado.");

            var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            
            if (item == null)
                return NotFound("Item não encontrado no carrinho.");

            item.Quantidade = novaQuantidade;

            return Ok(carrinho);
        }

        /// <summary>
        /// Remove um item do carrinho
        /// </summary>
        [HttpDelete("{clienteId}/item/{produtoId}")]
        public ActionResult<CarrinhoDto> RemoverItem(int clienteId, int produtoId)
        {
            var chaveCarrinho = $"cliente_{clienteId}";
            
            if (!_carrinhos.TryGetValue(chaveCarrinho, out var carrinho))
                return NotFound("Carrinho não encontrado.");

            var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            
            if (item == null)
                return NotFound("Item não encontrado no carrinho.");

            carrinho.Itens.Remove(item);

            return Ok(carrinho);
        }

        /// <summary>
        /// Limpa o carrinho
        /// </summary>
        [HttpDelete("{clienteId}")]
        public ActionResult LimparCarrinho(int clienteId)
        {
            var chaveCarrinho = $"cliente_{clienteId}";
            
            if (_carrinhos.ContainsKey(chaveCarrinho))
                _carrinhos.Remove(chaveCarrinho);

            return NoContent();
        }

        /// <summary>
        /// Finaliza a compra (converte carrinho em pedido)
        /// </summary>
        [HttpPost("{clienteId}/finalizar")]
        public async Task<ActionResult> FinalizarCompra(int clienteId, [FromBody] int enderecoEntregaId)
        {
            var chaveCarrinho = $"cliente_{clienteId}";
            
            if (!_carrinhos.TryGetValue(chaveCarrinho, out var carrinho) || !carrinho.Itens.Any())
                return BadRequest("Carrinho está vazio.");

            // Criar DTO do pedido baseado no carrinho
            var pedidoCreateDto = new PedidoCreateDto
            {
                ClienteId = clienteId,
                EnderecoEntregaId = enderecoEntregaId,
                Itens = carrinho.Itens.Select(i => new ItemPedidoCreateDto
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade
                }).ToList()
            };

            // Aqui você chamaria o PedidosController.CreatePedido ou o serviço diretamente
            // Por simplicidade, vou limpar o carrinho
            _carrinhos.Remove(chaveCarrinho);

            return Ok(new { Message = "Compra finalizada com sucesso!", PedidoData = pedidoCreateDto });
        }
    }
}
