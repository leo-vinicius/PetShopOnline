using Microsoft.AspNetCore.Mvc;
using PetShopAPI.DTOs;
using PetShopAPI.Models;
using PetShopAPI.Repositories;

namespace PetShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriasController(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        /// <summary>
        /// Obtém todas as categorias
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            
            var categoriasDto = categorias.Select(c => new CategoriaDto
            {
                IdCategoria = c.IdCategoria,
                Nome = c.Nome
            });

            return Ok(categoriasDto);
        }

        /// <summary>
        /// Obtém uma categoria específica por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDto>> GetCategoria(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);

            if (categoria == null)
                return NotFound($"Categoria com ID {id} não encontrada.");

            var categoriaDto = new CategoriaDto
            {
                IdCategoria = categoria.IdCategoria,
                Nome = categoria.Nome
            };

            return Ok(categoriaDto);
        }

        /// <summary>
        /// Cria uma nova categoria (Admin)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<CategoriaDto>> CreateCategoria(CategoriaCreateDto categoriaCreateDto)
        {
            var categoria = new Categoria
            {
                Nome = categoriaCreateDto.Nome
            };

            await _categoriaRepository.CreateAsync(categoria);

            var categoriaDto = new CategoriaDto
            {
                IdCategoria = categoria.IdCategoria,
                Nome = categoria.Nome
            };

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.IdCategoria }, categoriaDto);
        }

        /// <summary>
        /// Atualiza uma categoria existente (Admin)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, CategoriaCreateDto categoriaUpdateDto)
        {
            if (!await _categoriaRepository.ExistsAsync(id))
                return NotFound($"Categoria com ID {id} não encontrada.");

            var categoria = await _categoriaRepository.GetByIdAsync(id);
            categoria!.Nome = categoriaUpdateDto.Nome;

            await _categoriaRepository.UpdateAsync(categoria);

            return NoContent();
        }

        /// <summary>
        /// Remove uma categoria (Admin)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            if (!await _categoriaRepository.ExistsAsync(id))
                return NotFound($"Categoria com ID {id} não encontrada.");

            await _categoriaRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
