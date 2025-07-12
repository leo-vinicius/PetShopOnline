using Microsoft.EntityFrameworkCore;
using PetShopAPI.Models;

namespace PetShopAPI.Data
{
    public class PetShopContext : DbContext
    {
        public PetShopContext(DbContextOptions<PetShopContext> options) : base(options)
        {
        }

        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<ProdutoCategoria> ProdutoCategorias { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure table names
            modelBuilder.Entity<Administrador>().ToTable("administrador");
            modelBuilder.Entity<Categoria>().ToTable("categoria");
            modelBuilder.Entity<Produto>().ToTable("produto");
            modelBuilder.Entity<Cliente>().ToTable("cliente");
            modelBuilder.Entity<Endereco>().ToTable("endereco");
            modelBuilder.Entity<Pedido>().ToTable("pedido");
            modelBuilder.Entity<ItemPedido>().ToTable("itemPedido");
            modelBuilder.Entity<ProdutoCategoria>().ToTable("produtoCategoria");
            modelBuilder.Entity<Favorito>().ToTable("favorito");

            // Configure unique constraints
            modelBuilder.Entity<Administrador>()
                .HasIndex(a => a.Email)
                .IsUnique();

            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Email)
                .IsUnique();

            // Configure relationships
            // Primary category relationship (maintains backward compatibility)
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Many-to-many: Produto <-> Categoria (RN01 - additional categories)
            modelBuilder.Entity<ProdutoCategoria>()
                .HasOne(pc => pc.Produto)
                .WithMany(p => p.ProdutoCategorias)
                .HasForeignKey(pc => pc.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProdutoCategoria>()
                .HasOne(pc => pc.Categoria)
                .WithMany(c => c.ProdutoCategorias)
                .HasForeignKey(pc => pc.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint: Um produto não pode ter a mesma categoria duplicada
            modelBuilder.Entity<ProdutoCategoria>()
                .HasIndex(pc => new { pc.ProdutoId, pc.CategoriaId })
                .IsUnique();

            // Favoritos relationship (RF12)
            modelBuilder.Entity<Favorito>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.Favoritos)
                .HasForeignKey(f => f.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorito>()
                .HasOne(f => f.Produto)
                .WithMany(p => p.Favoritos)
                .HasForeignKey(f => f.ProdutoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint: Um cliente não pode favoritar o mesmo produto duas vezes
            modelBuilder.Entity<Favorito>()
                .HasIndex(f => new { f.ClienteId, f.ProdutoId })
                .IsUnique();

            modelBuilder.Entity<Endereco>()
                .HasOne(e => e.Cliente)
                .WithMany(c => c.Enderecos)
                .HasForeignKey(e => e.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.EnderecoEntrega)
                .WithMany(e => e.Pedidos)
                .HasForeignKey(p => p.EnderecoEntregaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Produto)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure decimal precision
            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.ValorTotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.PrecoUnitario)
                .HasPrecision(10, 2);

            // Configure enum conversion
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Status)
                .HasConversion<string>();

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categorias
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, Nome = "Rações" },
                new Categoria { IdCategoria = 2, Nome = "Brinquedos" },
                new Categoria { IdCategoria = 3, Nome = "Medicamentos" }
            );

            // Seed Administradores
            modelBuilder.Entity<Administrador>().HasData(
                new Administrador { IdAdmin = 1, Nome = "Leonardo Soares", Email = "admin1@petshop.com", Senha = "admin123" },
                new Administrador { IdAdmin = 2, Nome = "Lucca Braga", Email = "admin2@petshop.com", Senha = "admin123" },
                new Administrador { IdAdmin = 3, Nome = "Luca Domingues", Email = "admin3@petshop.com", Senha = "admin123" }
            );

            // Seed Produtos (com CategoriaId para compatibilidade)
            modelBuilder.Entity<Produto>().HasData(
                new Produto
                {
                    IdProduto = 1,
                    Nome = "Ração Premium 10kg",
                    Descricao = "Ração para cães adultos",
                    Preco = 129.90m,
                    Estoque = 50,
                    CategoriaId = 1,
                    Ativo = true
                },
                new Produto
                {
                    IdProduto = 2,
                    Nome = "Osso de Nylon",
                    Descricao = "Brinquedo resistente para cães",
                    Preco = 39.90m,
                    Estoque = 100,
                    CategoriaId = 2,
                    Ativo = true
                },
                new Produto
                {
                    IdProduto = 3,
                    Nome = "Vermífugo PetLife",
                    Descricao = "Medicamento oral para vermes",
                    Preco = 24.50m,
                    Estoque = 30,
                    CategoriaId = 3,
                    Ativo = true
                }
            );

            // Seed ProdutoCategoria (many-to-many relationships)
            modelBuilder.Entity<ProdutoCategoria>().HasData(
                // Ração Premium - categoria "Rações"
                new ProdutoCategoria { IdProdutoCategoria = 1, ProdutoId = 1, CategoriaId = 1 },
                // Osso de Nylon - categoria "Brinquedos"
                new ProdutoCategoria { IdProdutoCategoria = 2, ProdutoId = 2, CategoriaId = 2 },
                // Vermífugo - categoria "Medicamentos"
                new ProdutoCategoria { IdProdutoCategoria = 3, ProdutoId = 3, CategoriaId = 3 }
            );
        }
    }
}
