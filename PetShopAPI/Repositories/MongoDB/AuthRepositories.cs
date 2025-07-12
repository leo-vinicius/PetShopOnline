using MongoDB.Driver;
using PetShopAPI.Data;
using PetShopAPI.Models.MongoDB;

namespace PetShopAPI.Repositories.MongoDB
{
    public interface IAdministradorMongoRepository
    {
        Task<AdministradorMongo?> GetByIdAsync(string id);
        Task<AdministradorMongo?> GetByEmailAsync(string email);
        Task<AdministradorMongo> CreateAsync(AdministradorMongo administrador);
        Task<AdministradorMongo> UpdateAsync(AdministradorMongo administrador);
        Task<bool> EmailExistsAsync(string email);
        Task UpdateLastLoginAsync(string id);
    }

    public class AdministradorMongoRepository : IAdministradorMongoRepository
    {
        private readonly MongoDbContext _context;

        public AdministradorMongoRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<AdministradorMongo?> GetByIdAsync(string id)
        {
            return await _context.Administradores
                .Find(a => a.Id == id && a.Ativo)
                .FirstOrDefaultAsync();
        }

        public async Task<AdministradorMongo?> GetByEmailAsync(string email)
        {
            return await _context.Administradores
                .Find(a => a.Email == email && a.Ativo)
                .FirstOrDefaultAsync();
        }

        public async Task<AdministradorMongo> CreateAsync(AdministradorMongo administrador)
        {
            await _context.Administradores.InsertOneAsync(administrador);
            return administrador;
        }

        public async Task<AdministradorMongo> UpdateAsync(AdministradorMongo administrador)
        {
            await _context.Administradores.ReplaceOneAsync(
                a => a.Id == administrador.Id,
                administrador);
            return administrador;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var count = await _context.Administradores
                .CountDocumentsAsync(a => a.Email == email && a.Ativo);
            return count > 0;
        }

        public async Task UpdateLastLoginAsync(string id)
        {
            var update = Builders<AdministradorMongo>.Update
                .Set(a => a.UltimoLogin, DateTime.UtcNow);

            await _context.Administradores.UpdateOneAsync(
                a => a.Id == id,
                update);
        }
    }

    public interface IClienteMongoRepository
    {
        Task<ClienteMongo?> GetByIdAsync(string id);
        Task<ClienteMongo?> GetByEmailAsync(string email);
        Task<ClienteMongo> CreateAsync(ClienteMongo cliente);
        Task<ClienteMongo> UpdateAsync(ClienteMongo cliente);
        Task<bool> EmailExistsAsync(string email);
        Task UpdateLastLoginAsync(string id);
        Task AddEnderecoAsync(string clienteId, EnderecoMongo endereco);
        Task UpdateEnderecoAsync(string clienteId, EnderecoMongo endereco);
        Task RemoveEnderecoAsync(string clienteId, string enderecoId);
    }

    public class ClienteMongoRepository : IClienteMongoRepository
    {
        private readonly MongoDbContext _context;

        public ClienteMongoRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<ClienteMongo?> GetByIdAsync(string id)
        {
            return await _context.Clientes
                .Find(c => c.Id == id && c.Ativo)
                .FirstOrDefaultAsync();
        }

        public async Task<ClienteMongo?> GetByEmailAsync(string email)
        {
            return await _context.Clientes
                .Find(c => c.Email == email && c.Ativo)
                .FirstOrDefaultAsync();
        }

        public async Task<ClienteMongo> CreateAsync(ClienteMongo cliente)
        {
            await _context.Clientes.InsertOneAsync(cliente);
            return cliente;
        }

        public async Task<ClienteMongo> UpdateAsync(ClienteMongo cliente)
        {
            await _context.Clientes.ReplaceOneAsync(
                c => c.Id == cliente.Id,
                cliente);
            return cliente;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var count = await _context.Clientes
                .CountDocumentsAsync(c => c.Email == email && c.Ativo);
            return count > 0;
        }

        public async Task UpdateLastLoginAsync(string id)
        {
            var update = Builders<ClienteMongo>.Update
                .Set(c => c.UltimoLogin, DateTime.UtcNow);

            await _context.Clientes.UpdateOneAsync(
                c => c.Id == id,
                update);
        }

        public async Task AddEnderecoAsync(string clienteId, EnderecoMongo endereco)
        {
            var update = Builders<ClienteMongo>.Update
                .Push(c => c.Enderecos, endereco);

            await _context.Clientes.UpdateOneAsync(
                c => c.Id == clienteId,
                update);
        }

        public async Task UpdateEnderecoAsync(string clienteId, EnderecoMongo endereco)
        {
            var filter = Builders<ClienteMongo>.Filter.And(
                Builders<ClienteMongo>.Filter.Eq(c => c.Id, clienteId),
                Builders<ClienteMongo>.Filter.ElemMatch(c => c.Enderecos, 
                    e => e.IdEndereco == endereco.IdEndereco));

            var update = Builders<ClienteMongo>.Update
                .Set("enderecos.$", endereco);

            await _context.Clientes.UpdateOneAsync(filter, update);
        }

        public async Task RemoveEnderecoAsync(string clienteId, string enderecoId)
        {
            var update = Builders<ClienteMongo>.Update
                .PullFilter(c => c.Enderecos, e => e.IdEndereco == enderecoId);

            await _context.Clientes.UpdateOneAsync(
                c => c.Id == clienteId,
                update);
        }
    }
}
