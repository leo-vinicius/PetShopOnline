using Microsoft.EntityFrameworkCore;
using PetShopAPI.Data;
using PetShopAPI.Models;

namespace PetShopAPI.Repositories
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> GetAllAsync(int? categoriaId = null, string? termo = null, bool? ativo = null);
        Task<Produto?> GetByIdAsync(int id);
        Task<Produto> CreateAsync(Produto produto);
        Task<Produto> UpdateAsync(Produto produto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }

    public class ProdutoRepository : IProdutoRepository
    {
        private readonly PetShopContext _context;

        public ProdutoRepository(PetShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Produto>> GetAllAsync(int? categoriaId = null, string? termo = null, bool? ativo = null)
        {
            var query = _context.Produtos
                .Include(p => p.Categoria)
                .AsQueryable();

            if (categoriaId.HasValue)
                query = query.Where(p => p.CategoriaId == categoriaId.Value);

            if (!string.IsNullOrEmpty(termo))
                query = query.Where(p => p.Nome.Contains(termo) ||
                                        (p.Descricao != null && p.Descricao.Contains(termo)));

            if (ativo.HasValue)
                query = query.Where(p => p.Ativo == ativo.Value);

            return await query.OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<Produto?> GetByIdAsync(int id)
        {
            return await _context.Produtos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.IdProduto == id);
        }

        public async Task<Produto> CreateAsync(Produto produto)
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task<Produto> UpdateAsync(Produto produto)
        {
            _context.Entry(produto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return produto;
        }

        public async Task DeleteAsync(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Produtos.AnyAsync(p => p.IdProduto == id);
        }
    }
}
