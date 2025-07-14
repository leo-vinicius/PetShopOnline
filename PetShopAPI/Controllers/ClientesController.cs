using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PetShopAPI.DTOs;
using PetShopAPI.Models;
using PetShopAPI.Repositories;

namespace PetShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public ClientesController(IClienteRepository clienteRepository, IEnderecoRepository enderecoRepository)
        {
            _clienteRepository = clienteRepository;
            _enderecoRepository = enderecoRepository;
        }

        /// <summary>
        /// Obtém um cliente por ID (apenas dados básicos para SQL Server)
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetCliente(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);

            if (cliente == null)
                return NotFound($"Cliente com ID {id} não encontrado.");

            var clienteDto = new ClienteDto
            {
                IdCliente = cliente.IdCliente,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone,
                Enderecos = cliente.Enderecos.Select(e => new EnderecoDto
                {
                    IdEndereco = e.IdEndereco,
                    Logradouro = e.Logradouro,
                    Numero = e.Numero,
                    Bairro = e.Bairro,
                    Cidade = e.Cidade,
                    Estado = e.Estado,
                    Cep = e.Cep,
                    ClienteId = e.ClienteId
                }).ToList()
            };

            return Ok(clienteDto);
        }

        /// <summary>
        /// Obtém todos os clientes (apenas dados básicos para SQL Server)
        /// </summary>
        public async Task<ActionResult<ClienteDto>> GetClientes()
        {
            var clientes = await _clienteRepository.GetAllAsync();

            if (clientes == null || !clientes.Any())
                return NotFound("Não há nenhum cliente registrado.");

            var clientesDto = clientes.Select(cliente => new ClienteDto
            {
                IdCliente = cliente.IdCliente,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone,
                Enderecos = cliente.Enderecos.Select(e => new EnderecoDto
                {
                    IdEndereco = e.IdEndereco,
                    Logradouro = e.Logradouro,
                    Numero = e.Numero,
                    Bairro = e.Bairro,
                    Cidade = e.Cidade,
                    Estado = e.Estado,
                    Cep = e.Cep,
                    ClienteId = e.ClienteId
                }).ToList()
            });

            return Ok(clientesDto);
        }

        /// <summary>
        /// Adiciona um endereço ao cliente
        /// </summary>
        [HttpPost("{clienteId}/enderecos")]
        public async Task<ActionResult<EnderecoDto>> AdicionarEndereco(int clienteId, EnderecoCreateDto enderecoCreateDto)
        {
            if (!await _clienteRepository.ExistsAsync(clienteId))
                return NotFound($"Cliente com ID {clienteId} não encontrado.");

            var endereco = new Endereco
            {
                Logradouro = enderecoCreateDto.Logradouro,
                Numero = enderecoCreateDto.Numero,
                Bairro = enderecoCreateDto.Bairro,
                Cidade = enderecoCreateDto.Cidade,
                Estado = enderecoCreateDto.Estado,
                Cep = enderecoCreateDto.Cep,
                ClienteId = clienteId
            };

            await _enderecoRepository.CreateAsync(endereco);

            var enderecoDto = new EnderecoDto
            {
                IdEndereco = endereco.IdEndereco,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado,
                Cep = endereco.Cep,
                ClienteId = endereco.ClienteId
            };

            return CreatedAtAction(nameof(GetEndereco), new { id = endereco.IdEndereco }, enderecoDto);
        }

        /// <summary>
        /// Obtém um endereço específico
        /// </summary>
        [HttpGet("enderecos/{id}")]
        public async Task<ActionResult<EnderecoDto>> GetEndereco(int id)
        {
            var endereco = await _enderecoRepository.GetByIdAsync(id);

            if (endereco == null)
                return NotFound($"Endereço com ID {id} não encontrado.");

            var enderecoDto = new EnderecoDto
            {
                IdEndereco = endereco.IdEndereco,
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado,
                Cep = endereco.Cep,
                ClienteId = endereco.ClienteId
            };

            return Ok(enderecoDto);
        }

        /// <summary>
        /// Obtém endereços de um cliente
        /// </summary>
        [HttpGet("{clienteId}/enderecos")]
        public async Task<ActionResult<IEnumerable<EnderecoDto>>> GetEnderecosByCliente(int clienteId)
        {
            if (!await _clienteRepository.ExistsAsync(clienteId))
                return NotFound($"Cliente com ID {clienteId} não encontrado.");

            var enderecos = await _enderecoRepository.GetByClienteIdAsync(clienteId);

            var enderecosDto = enderecos.Select(e => new EnderecoDto
            {
                IdEndereco = e.IdEndereco,
                Logradouro = e.Logradouro,
                Numero = e.Numero,
                Bairro = e.Bairro,
                Cidade = e.Cidade,
                Estado = e.Estado,
                Cep = e.Cep,
                ClienteId = e.ClienteId
            });

            return Ok(enderecosDto);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}
