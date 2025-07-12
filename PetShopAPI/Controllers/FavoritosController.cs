using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShopAPI.Data;
using PetShopAPI.DTOs;
using PetShopAPI.Models;
using System.Security.Claims;

namespace PetShopAPI.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de favoritos
    /// Implementa RF12: Sistema deverá permitir que o cliente possa favoritar produtos
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoritosController : ControllerBase
    {
        private readonly PetShopContext _context;

        public FavoritosController(PetShopContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os favoritos do cliente logado
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<FavoritoDto>>> GetFavoritos()
        {
            var clienteId = GetClienteId();
            if (clienteId == null)
                return Unauthorized("Cliente não identificado");

            var favoritos = await _context.Favoritos
                .Include(f => f.Produto)
                .Where(f => f.ClienteId == clienteId)
                .Select(f => new FavoritoDto
                {
                    IdFavorito = f.IdFavorito,
                    ClienteId = f.ClienteId,
                    ProdutoId = f.ProdutoId,
                    ProdutoNome = f.Produto.Nome,
                    ProdutoPreco = f.Produto.Preco,
                    ProdutoImagemUrl = f.Produto.ImagemUrl,
                    DataFavorito = f.DataFavorito
                })
                .ToListAsync();

            return Ok(favoritos);
        }

        /// <summary>
        /// Adiciona um produto aos favoritos
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FavoritoDto>> AdicionarFavorito([FromBody] AdicionarFavoritoDto dto)
        {
            var clienteId = GetClienteId();
            if (clienteId == null)
                return Unauthorized("Cliente não identificado");

            // Verificar se o produto existe
            var produto = await _context.Produtos.FindAsync(dto.ProdutoId);
            if (produto == null)
                return NotFound("Produto não encontrado");

            // Verificar se já não está nos favoritos
            var favoritoExistente = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.ClienteId == clienteId && f.ProdutoId == dto.ProdutoId);

            if (favoritoExistente != null)
                return BadRequest("Produto já está nos favoritos");

            var favorito = new Favorito
            {
                ClienteId = clienteId.Value,
                ProdutoId = dto.ProdutoId,
                DataFavorito = DateTime.UtcNow
            };

            _context.Favoritos.Add(favorito);
            await _context.SaveChangesAsync();

            var favoritoDto = new FavoritoDto
            {
                IdFavorito = favorito.IdFavorito,
                ClienteId = favorito.ClienteId,
                ProdutoId = favorito.ProdutoId,
                ProdutoNome = produto.Nome,
                ProdutoPreco = produto.Preco,
                ProdutoImagemUrl = produto.ImagemUrl,
                DataFavorito = favorito.DataFavorito
            };

            return CreatedAtAction(nameof(GetFavoritos), favoritoDto);
        }

        /// <summary>
        /// Remove um produto dos favoritos
        /// </summary>
        [HttpDelete("{produtoId}")]
        public async Task<IActionResult> RemoverFavorito(int produtoId)
        {
            var clienteId = GetClienteId();
            if (clienteId == null)
                return Unauthorized("Cliente não identificado");

            var favorito = await _context.Favoritos
                .FirstOrDefaultAsync(f => f.ClienteId == clienteId && f.ProdutoId == produtoId);

            if (favorito == null)
                return NotFound("Favorito não encontrado");

            _context.Favoritos.Remove(favorito);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Verifica se um produto está nos favoritos do cliente
        /// </summary>
        [HttpGet("verificar/{produtoId}")]
        public async Task<ActionResult<bool>> VerificarFavorito(int produtoId)
        {
            var clienteId = GetClienteId();
            if (clienteId == null)
                return Unauthorized("Cliente não identificado");

            var isFavorito = await _context.Favoritos
                .AnyAsync(f => f.ClienteId == clienteId && f.ProdutoId == produtoId);

            return Ok(isFavorito);
        }

        private int? GetClienteId()
        {
            var clienteIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(clienteIdClaim, out int clienteId))
                return clienteId;
            return null;
        }
    }
}
