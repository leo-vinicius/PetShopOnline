using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Data;
using Microsoft.Extensions.Caching.Memory;

namespace PetShopPro.Api.Features.Carrinho;

public interface ICarrinhoService
{
    Task<CarrinhoDto> GetCarrinho(int clienteId);
    Task<bool> AdicionarItem(int clienteId, int produtoId, int quantidade);
    Task<bool> AtualizarQuantidade(int clienteId, int produtoId, int quantidade);
    Task<bool> RemoverItem(int clienteId, int produtoId);
    Task LimparCarrinho(int clienteId);
}

public class CarrinhoService : ICarrinhoService
{
    private readonly PetShopContext _context;
    private readonly IMemoryCache _cache;

    public CarrinhoService(PetShopContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    private string GetCarrinhoKey(int clienteId) => $"carrinho_{clienteId}";

    private Dictionary<int, int> GetCarrinhoFromCache(int clienteId)
    {
        var key = GetCarrinhoKey(clienteId);
        return _cache.GetOrCreate(key, factory => new Dictionary<int, int>()) ?? new Dictionary<int, int>();
    }

    private void SaveCarrinhoToCache(int clienteId, Dictionary<int, int> carrinho)
    {
        var key = GetCarrinhoKey(clienteId);
        _cache.Set(key, carrinho, TimeSpan.FromHours(24)); // Expira em 24 horas
    }
    public async Task<CarrinhoDto> GetCarrinho(int clienteId)
    {
        var itens = GetCarrinhoFromCache(clienteId);

        if (!itens.Any())
            return new CarrinhoDto(new List<ItemCarrinhoDto>(), 0, 0);

        var produtoIds = itens.Keys.ToList();

        var produtos = await _context.Produtos
            .Include(p => p.Categoria)
            .Where(p => produtoIds.Contains(p.Id) && p.Ativo && p.Categoria.Ativo)
            .ToListAsync();

        var itemsCarrinho = new List<ItemCarrinhoDto>();

        foreach (var item in itens)
        {
            var produto = produtos.FirstOrDefault(p => p.Id == item.Key);
            if (produto != null)
            {
                var quantidade = Math.Min(item.Value, produto.Estoque); // Não permitir mais que o estoque
                var subtotal = produto.Preco * quantidade;

                itemsCarrinho.Add(new ItemCarrinhoDto(
                    produto.Id,
                    produto.Nome,
                    produto.Preco,
                    quantidade,
                    subtotal,
                    produto.Estoque
                ));
            }
        }

        var total = itemsCarrinho.Sum(i => i.Subtotal);
        var totalItems = itemsCarrinho.Sum(i => i.Quantidade);

        return new CarrinhoDto(itemsCarrinho, total, totalItems);
    }
    public async Task<bool> AdicionarItem(int clienteId, int produtoId, int quantidade)
    {
        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == produtoId && p.Ativo);

        if (produto == null || quantidade <= 0 || quantidade > produto.Estoque)
            return false;

        var carrinho = GetCarrinhoFromCache(clienteId);

        if (carrinho.ContainsKey(produtoId))
        {
            var novaQuantidade = carrinho[produtoId] + quantidade;
            if (novaQuantidade > produto.Estoque)
                return false;

            carrinho[produtoId] = novaQuantidade;
        }
        else
        {
            carrinho[produtoId] = quantidade;
        }

        SaveCarrinhoToCache(clienteId, carrinho);
        return true;
    }

    public async Task<bool> AtualizarQuantidade(int clienteId, int produtoId, int quantidade)
    {
        var carrinho = GetCarrinhoFromCache(clienteId);

        if (!carrinho.ContainsKey(produtoId))
            return false;

        if (quantidade <= 0)
        {
            carrinho.Remove(produtoId);
            SaveCarrinhoToCache(clienteId, carrinho);
            return true;
        }

        var produto = await _context.Produtos
            .FirstOrDefaultAsync(p => p.Id == produtoId && p.Ativo);

        if (produto == null || quantidade > produto.Estoque)
            return false;

        carrinho[produtoId] = quantidade;
        SaveCarrinhoToCache(clienteId, carrinho);
        return true;
    }

    public Task<bool> RemoverItem(int clienteId, int produtoId)
    {
        var carrinho = GetCarrinhoFromCache(clienteId);

        if (carrinho.ContainsKey(produtoId))
        {
            carrinho.Remove(produtoId);
            SaveCarrinhoToCache(clienteId, carrinho);
        }

        return Task.FromResult(true);
    }

    public Task LimparCarrinho(int clienteId)
    {
        var carrinho = new Dictionary<int, int>();
        SaveCarrinhoToCache(clienteId, carrinho);
        return Task.CompletedTask;
    }
}
