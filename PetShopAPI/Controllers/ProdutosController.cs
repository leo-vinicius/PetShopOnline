using Microsoft.AspNetCore.Mvc;
using PetShopAPI.DTOs;
using PetShopAPI.Models;
using PetShopAPI.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace PetShopAPI.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de produtos do PetShop
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Gestão de produtos para animais de estimação")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public ProdutosController(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
        }

        /// <summary>
        /// Obtém lista de produtos com filtros opcionais
        /// </summary>
        /// <param name="categoriaId">Filtrar por ID da categoria (opcional)</param>
        /// <param name="termo">Termo para busca no nome ou descrição (opcional)</param>
        /// <param name="ativo">Filtrar apenas produtos ativos (opcional)</param>
        /// <returns>Lista de produtos filtrados</returns>
        /// <response code="200">Lista de produtos retornada com sucesso</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista produtos com filtros",
            Description = "Retorna uma lista de produtos com opções de filtro por categoria, termo de busca e status ativo/inativo.",
            OperationId = "GetProdutos",
            Tags = new[] { "Produtos" }
        )]
        [SwaggerResponse(200, "Lista de produtos retornada com sucesso", typeof(IEnumerable<ProdutoDto>))]
        [SwaggerResponse(500, "Erro interno do servidor")]
        public async Task<ActionResult<IEnumerable<ProdutoDto>>> GetProdutos(
            [FromQuery] int? categoriaId = null,
            [FromQuery] string? termo = null,
            [FromQuery] bool? ativo = null)
        {
            var produtos = await _produtoRepository.GetAllAsync(categoriaId, termo, ativo);

            var produtosDto = produtos.Select(p => new ProdutoDto
            {
                IdProduto = p.IdProduto,
                Nome = p.Nome,
                Descricao = p.Descricao,
                Preco = p.Preco,
                Estoque = p.Estoque,
                Ativo = p.Ativo,
                ImagemUrl = p.ImagemUrl,
                CategoriaId = p.CategoriaId,
                CategoriaNome = p.Categoria.Nome
            });

            return Ok(produtosDto);
        }

        /// <summary>
        /// Obtém um produto específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoDto>> GetProduto(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);

            if (produto == null)
                return NotFound($"Produto com ID {id} não encontrado.");

            var produtoDto = new ProdutoDto
            {
                IdProduto = produto.IdProduto,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Estoque = produto.Estoque,
                Ativo = produto.Ativo,
                ImagemUrl = produto.ImagemUrl,
                CategoriaId = produto.CategoriaId,
                CategoriaNome = produto.Categoria.Nome
            };

            return Ok(produtoDto);
        }

        /// <summary>
        /// Cria um novo produto (Admin)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProdutoDto>> CreateProduto(ProdutoCreateDto produtoCreateDto)
        {
            // Verificar se a categoria existe
            if (!await _categoriaRepository.ExistsAsync(produtoCreateDto.CategoriaId))
                return BadRequest($"Categoria com ID {produtoCreateDto.CategoriaId} não encontrada.");

            var produto = new Produto
            {
                Nome = produtoCreateDto.Nome,
                Descricao = produtoCreateDto.Descricao,
                Preco = produtoCreateDto.Preco,
                Estoque = produtoCreateDto.Estoque,
                Ativo = produtoCreateDto.Ativo,
                ImagemUrl = produtoCreateDto.ImagemUrl,
                CategoriaId = produtoCreateDto.CategoriaId
            };

            await _produtoRepository.CreateAsync(produto);

            // Recarregar com categoria para retorno
            produto = await _produtoRepository.GetByIdAsync(produto.IdProduto);

            var produtoDto = new ProdutoDto
            {
                IdProduto = produto!.IdProduto,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Preco = produto.Preco,
                Estoque = produto.Estoque,
                Ativo = produto.Ativo,
                ImagemUrl = produto.ImagemUrl,
                CategoriaId = produto.CategoriaId,
                CategoriaNome = produto.Categoria.Nome
            };

            return CreatedAtAction(nameof(GetProduto), new { id = produto.IdProduto }, produtoDto);
        }

        /// <summary>
        /// Atualiza um produto existente (Admin)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduto(int id, ProdutoUpdateDto produtoUpdateDto)
        {
            if (!await _produtoRepository.ExistsAsync(id))
                return NotFound($"Produto com ID {id} não encontrado.");

            if (!await _categoriaRepository.ExistsAsync(produtoUpdateDto.CategoriaId))
                return BadRequest($"Categoria com ID {produtoUpdateDto.CategoriaId} não encontrada.");

            var produto = await _produtoRepository.GetByIdAsync(id);
            produto!.Nome = produtoUpdateDto.Nome;
            produto.Descricao = produtoUpdateDto.Descricao;
            produto.Preco = produtoUpdateDto.Preco;
            produto.Estoque = produtoUpdateDto.Estoque;
            produto.Ativo = produtoUpdateDto.Ativo;
            produto.ImagemUrl = produtoUpdateDto.ImagemUrl;
            produto.CategoriaId = produtoUpdateDto.CategoriaId;

            await _produtoRepository.UpdateAsync(produto);

            return NoContent();
        }

        /// <summary>
        /// Remove um produto logicamente (Admin) - Define como inativo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
                return NotFound($"Produto com ID {id} não encontrado.");

            // Soft delete - define produto como inativo ao invés de remover fisicamente
            produto.Ativo = false;
            await _produtoRepository.UpdateAsync(produto);

            return NoContent();
        }

        /// <summary>
        /// Atualiza apenas o estoque de um produto (Admin)
        /// </summary>
        [HttpPatch("{id}/estoque")]
        public async Task<IActionResult> UpdateEstoque(int id, [FromBody] int novoEstoque)
        {
            if (novoEstoque < 0)
                return BadRequest("Estoque não pode ser negativo.");

            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
                return NotFound($"Produto com ID {id} não encontrado.");

            produto.Estoque = novoEstoque;
            await _produtoRepository.UpdateAsync(produto);

            return NoContent();
        }
    }
}
