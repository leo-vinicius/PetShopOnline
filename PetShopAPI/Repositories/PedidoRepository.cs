using Microsoft.EntityFrameworkCore;
using PetShopAPI.Data;
using PetShopAPI.Models;

namespace PetShopAPI.Repositories
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> GetAllAsync();
        Task<IEnumerable<Pedido>> GetByClienteIdAsync(int clienteId);
        Task<Pedido?> GetByIdAsync(int id);
        Task<Pedido> CreateAsync(Pedido pedido);
        Task<Pedido> UpdateAsync(Pedido pedido);
        Task<bool> ExistsAsync(int id);
    }

    public class PedidoRepository : IPedidoRepository
    {
        private readonly PetShopContext _context;

        public PedidoRepository(PetShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pedido>> GetAllAsync()
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.EnderecoEntrega)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.EnderecoEntrega)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Where(p => p.ClienteId == clienteId)
                .OrderByDescending(p => p.DataPedido)
                .ToListAsync();
        }

        public async Task<Pedido?> GetByIdAsync(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.EnderecoEntrega)
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .FirstOrDefaultAsync(p => p.IdPedido == id);
        }

        public async Task<Pedido> CreateAsync(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> UpdateAsync(Pedido pedido)
        {
            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Pedidos.AnyAsync(p => p.IdPedido == id);
        }
    }

    public interface IEnderecoRepository
    {
        Task<IEnumerable<Endereco>> GetByClienteIdAsync(int clienteId);
        Task<Endereco?> GetByIdAsync(int id);
        Task<Endereco> CreateAsync(Endereco endereco);
        Task<Endereco> UpdateAsync(Endereco endereco);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }

    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly PetShopContext _context;

        public EnderecoRepository(PetShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Endereco>> GetByClienteIdAsync(int clienteId)
        {
            return await _context.Enderecos
                .Where(e => e.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<Endereco?> GetByIdAsync(int id)
        {
            return await _context.Enderecos.FindAsync(id);
        }

        public async Task<Endereco> CreateAsync(Endereco endereco)
        {
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();
            return endereco;
        }

        public async Task<Endereco> UpdateAsync(Endereco endereco)
        {
            _context.Entry(endereco).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return endereco;
        }

        public async Task DeleteAsync(int id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco != null)
            {
                _context.Enderecos.Remove(endereco);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Enderecos.AnyAsync(e => e.IdEndereco == id);
        }
    }
}
