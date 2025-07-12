using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using PetShopAPI.Data;
using PetShopAPI.Repositories;
using PetShopAPI.Repositories.MongoDB;
using PetShopAPI.Services;
using PetShopAPI.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure SQL Server Entity Framework for main data
builder.Services.AddDbContext<PetShopContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

// Configure MongoDB for authentication
builder.Services.AddSingleton<MongoDbContext>();

// Register SQL Server repositories
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();

// Register MongoDB repositories
builder.Services.AddScoped<IClienteMongoRepository, ClienteMongoRepository>();
builder.Services.AddScoped<IAdministradorMongoRepository, AdministradorMongoRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();

// Register services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();

// Configure Swagger with enhanced .NET 9 features
builder.Services.AddSwaggerConfiguration();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure JSON options for .NET 9
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline with enhanced Swagger for .NET 9
app.UseSwaggerConfiguration(app.Environment);

// Enable static files for custom Swagger assets
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

// Create database and apply migrations
using (var scope = app.Services.CreateScope())
{
    // Initialize SQL Server database
    var sqlContext = scope.ServiceProvider.GetRequiredService<PetShopContext>();
    try
    {
        sqlContext.Database.EnsureCreated();
        Console.WriteLine("✅ SQL Server database initialized successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error initializing SQL Server database: {ex.Message}");
    }

    // Test MongoDB connection
    var mongoContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    try
    {
        // Test connection by trying to access a collection
        var testCollection = mongoContext.Clientes;
        await testCollection.CountDocumentsAsync(_ => true);
        Console.WriteLine("✅ MongoDB connection established successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error connecting to MongoDB: {ex.Message}");
    }
}

Console.WriteLine("🚀 PetShop Hybrid API started successfully!");
Console.WriteLine("📊 SQL Server: Products, Orders, Categories (5CG5123HJ2\\SQLEXPRESS)");
Console.WriteLine("🍃 MongoDB: Authentication, Admin, Sessions (localhost:27017)");
Console.WriteLine("📚 Swagger UI available at: https://localhost:7001/");
Console.WriteLine("🔗 API Base URL: https://localhost:7001/api/");

app.Run();
