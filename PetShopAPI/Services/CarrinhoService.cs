using PetShopAPI.DTOs;

namespace PetShopAPI.Services
{
    /// <summary>
    /// Serviço para gerenciamento do carrinho de compras
    /// Implementa RN08: Carrinho deve ser esvaziado após finalização do pedido
    /// </summary>
    public interface ICarrinhoService
    {
        CarrinhoDto GetCarrinho(int clienteId);
        Task<bool> AdicionarItem(int clienteId, int produtoId, int quantidade);
        Task<bool> RemoverItem(int clienteId, int produtoId);
        Task<bool> AtualizarQuantidade(int clienteId, int produtoId, int novaQuantidade);
        void LimparCarrinho(int clienteId); // RN08
    }

    public class CarrinhoService : ICarrinhoService
    {
        private static readonly Dictionary<string, CarrinhoDto> _carrinhos = new();
        private readonly object _lock = new object();

        public CarrinhoDto GetCarrinho(int clienteId)
        {
            var chaveCarrinho = $"cliente_{clienteId}";

            lock (_lock)
            {
                if (_carrinhos.TryGetValue(chaveCarrinho, out var carrinho))
                    return carrinho;

                return new CarrinhoDto();
            }
        }

        public async Task<bool> AdicionarItem(int clienteId, int produtoId, int quantidade)
        {
            var chaveCarrinho = $"cliente_{clienteId}";

            lock (_lock)
            {
                if (!_carrinhos.ContainsKey(chaveCarrinho))
                    _carrinhos[chaveCarrinho] = new CarrinhoDto();

                var carrinho = _carrinhos[chaveCarrinho];
                var itemExistente = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);

                if (itemExistente != null)
                {
                    itemExistente.Quantidade += quantidade;
                }
                else
                {
                    // Note: Na implementação real, você precisa buscar os dados do produto
                    // Este é um exemplo simplificado
                    carrinho.Itens.Add(new ItemCarrinhoDto
                    {
                        ProdutoId = produtoId,
                        Quantidade = quantidade
                        // Outros campos serão preenchidos pelo controller
                    });
                }

                return true;
            }
        }

        public async Task<bool> RemoverItem(int clienteId, int produtoId)
        {
            var chaveCarrinho = $"cliente_{clienteId}";

            lock (_lock)
            {
                if (_carrinhos.TryGetValue(chaveCarrinho, out var carrinho))
                {
                    var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
                    if (item != null)
                    {
                        carrinho.Itens.Remove(item);
                        return true;
                    }
                }
                return false;
            }
        }

        public async Task<bool> AtualizarQuantidade(int clienteId, int produtoId, int novaQuantidade)
        {
            if (novaQuantidade <= 0)
                return await RemoverItem(clienteId, produtoId);

            var chaveCarrinho = $"cliente_{clienteId}";

            lock (_lock)
            {
                if (_carrinhos.TryGetValue(chaveCarrinho, out var carrinho))
                {
                    var item = carrinho.Itens.FirstOrDefault(i => i.ProdutoId == produtoId);
                    if (item != null)
                    {
                        item.Quantidade = novaQuantidade;
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Limpa o carrinho do cliente após finalização do pedido
        /// Implementa RN08: Carrinho automaticamente esvaziado após finalização
        /// </summary>
        public void LimparCarrinho(int clienteId)
        {
            var chaveCarrinho = $"cliente_{clienteId}";

            lock (_lock)
            {
                _carrinhos.Remove(chaveCarrinho);
            }
        }
    }
}
