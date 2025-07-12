using PetShopAPI.Models.MongoDB;
using PetShopAPI.Repositories.MongoDB;
using PetShopAPI.Repositories;
using PetShopAPI.Models;
using System.Security.Cryptography;
using System.Text;
using MongoDB.Driver;

namespace PetShopAPI.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? Token, string? UserId, string? UserType, string? Message)> LoginClienteAsync(string email, string senha);
        Task<(bool Success, string? Token, string? UserId, string? UserType, string? Message)> LoginAdministradorAsync(string email, string senha);
        Task<(bool Success, string? UserId, string? Message)> RegistrarClienteAsync(string nome, string email, string senha, string? telefone);
        Task<(bool Success, string? UserId, string? Message)> RegistrarAdministradorAsync(string nome, string email, string senha);
        Task<bool> ValidateTokenAsync(string token);
        Task LogoutAsync(string token);
        string GenerateToken();
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }

    public class AuthService : IAuthService
    {
        private readonly IClienteMongoRepository _clienteRepository;
        private readonly IAdministradorMongoRepository _administradorRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IClienteRepository _clienteSqlRepository;

        public AuthService(
            IClienteMongoRepository clienteRepository,
            IAdministradorMongoRepository administradorRepository,
            ISessionRepository sessionRepository,
            IClienteRepository clienteSqlRepository)
        {
            _clienteRepository = clienteRepository;
            _administradorRepository = administradorRepository;
            _sessionRepository = sessionRepository;
            _clienteSqlRepository = clienteSqlRepository;
        }

        public async Task<(bool Success, string? Token, string? UserId, string? UserType, string? Message)> LoginClienteAsync(string email, string senha)
        {
            var cliente = await _clienteRepository.GetByEmailAsync(email);

            if (cliente == null || !VerifyPassword(senha, cliente.Senha))
                return (false, null, null, null, "Email ou senha inválidos.");

            if (!cliente.Ativo)
                return (false, null, null, null, "Conta desativada.");

            var token = GenerateToken();
            var session = new SessionMongo
            {
                UserId = cliente.Id!,
                UserType = "cliente",
                Token = token,
                DataExpiracao = DateTime.UtcNow.AddHours(24)
            };

            await _sessionRepository.CreateAsync(session);
            await _clienteRepository.UpdateLastLoginAsync(cliente.Id!);

            return (true, token, cliente.Id, "cliente", "Login realizado com sucesso.");
        }

        public async Task<(bool Success, string? Token, string? UserId, string? UserType, string? Message)> LoginAdministradorAsync(string email, string senha)
        {
            var administrador = await _administradorRepository.GetByEmailAsync(email);

            if (administrador == null || !VerifyPassword(senha, administrador.Senha))
                return (false, null, null, null, "Email ou senha inválidos.");

            if (!administrador.Ativo)
                return (false, null, null, null, "Conta desativada.");

            var token = GenerateToken();
            var session = new SessionMongo
            {
                UserId = administrador.Id!,
                UserType = "administrador",
                Token = token,
                DataExpiracao = DateTime.UtcNow.AddHours(8) // Sessão mais curta para admin
            };

            await _sessionRepository.CreateAsync(session);
            await _administradorRepository.UpdateLastLoginAsync(administrador.Id!);

            return (true, token, administrador.Id, "administrador", "Login de administrador realizado com sucesso.");
        }

        public async Task<(bool Success, string? UserId, string? Message)> RegistrarClienteAsync(string nome, string email, string senha, string? telefone)
        {
            // Verificar se o email já existe no MongoDB
            if (await _clienteRepository.EmailExistsAsync(email))
                return (false, null, "Email já está em uso.");

            // Verificar se o email já existe no SQL Server
            if (await _clienteSqlRepository.EmailExistsAsync(email))
                return (false, null, "Email já está em uso.");

            try
            {
                // Criar no MongoDB (para autenticação)
                var clienteMongo = new ClienteMongo
                {
                    Nome = nome,
                    Email = email,
                    Senha = HashPassword(senha),
                    Telefone = telefone
                };

                await _clienteRepository.CreateAsync(clienteMongo);

                // Criar no SQL Server (para dados de negócio)
                var clienteSql = new Cliente
                {
                    Nome = nome,
                    Email = email,
                    Senha = HashPassword(senha),
                    Telefone = telefone
                };

                await _clienteSqlRepository.CreateAsync(clienteSql);

                return (true, clienteMongo.Id, "Cliente registrado com sucesso.");
            }
            catch (Exception ex)
            {
                // Em caso de erro, tentar fazer rollback se possível
                // Log do erro seria importante aqui
                return (false, null, "Erro interno ao registrar cliente.");
            }
        }

        public async Task<(bool Success, string? UserId, string? Message)> RegistrarAdministradorAsync(string nome, string email, string senha)
        {
            if (await _administradorRepository.EmailExistsAsync(email))
                return (false, null, "Email já está em uso.");

            var administrador = new AdministradorMongo
            {
                Nome = nome,
                Email = email,
                Senha = HashPassword(senha),
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            };

            await _administradorRepository.CreateAsync(administrador);
            return (true, administrador.Id, "Administrador registrado com sucesso.");
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var session = await _sessionRepository.GetByTokenAsync(token);
            return session != null && session.Ativo && session.DataExpiracao > DateTime.UtcNow;
        }

        public async Task LogoutAsync(string token)
        {
            await _sessionRepository.DeactivateByTokenAsync(token);
        }

        public string GenerateToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var token = new string(Enumerable.Repeat(chars, 64)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"{DateTime.UtcNow.Ticks}_{token}";
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }

    public interface ISessionRepository
    {
        Task<SessionMongo> CreateAsync(SessionMongo session);
        Task<SessionMongo?> GetByTokenAsync(string token);
        Task DeactivateByTokenAsync(string token);
        Task CleanupExpiredAsync();
    }

    public class SessionRepository : ISessionRepository
    {
        private readonly Data.MongoDbContext _context;

        public SessionRepository(Data.MongoDbContext context)
        {
            _context = context;
        }

        public async Task<SessionMongo> CreateAsync(SessionMongo session)
        {
            await _context.Sessions.InsertOneAsync(session);
            return session;
        }

        public async Task<SessionMongo?> GetByTokenAsync(string token)
        {
            return await _context.Sessions
                .Find(s => s.Token == token && s.Ativo && s.DataExpiracao > DateTime.UtcNow)
                .FirstOrDefaultAsync();
        }

        public async Task DeactivateByTokenAsync(string token)
        {
            var update = Builders<SessionMongo>.Update
                .Set(s => s.Ativo, false);

            await _context.Sessions.UpdateOneAsync(
                s => s.Token == token,
                update);
        }

        public async Task CleanupExpiredAsync()
        {
            await _context.Sessions.DeleteManyAsync(
                s => !s.Ativo || s.DataExpiracao <= DateTime.UtcNow);
        }
    }
}
