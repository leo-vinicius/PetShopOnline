using Microsoft.EntityFrameworkCore;
using PetShopPro.Api.Infrastructure.Entities;

namespace PetShopPro.Api.Infrastructure.Data;

public class PetShopContext : DbContext
{
    public PetShopContext(DbContextOptions<PetShopContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Administrador> Administradores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações de índices únicos
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Administrador>()
            .HasIndex(a => a.Email)
            .IsUnique();

        // Configurações de relacionamentos
        modelBuilder.Entity<Produto>()
            .HasOne(p => p.Categoria)
            .WithMany(c => c.Produtos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Endereco>()
            .HasOne(e => e.Cliente)
            .WithMany(c => c.Enderecos)
            .HasForeignKey(e => e.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configurações de precisão decimal
        modelBuilder.Entity<Produto>()
            .Property(p => p.Preco)
            .HasPrecision(10, 2);
    }
}
