using MongoDB.Driver;
using PetShopPro.Api.Infrastructure.Documents;

namespace PetShopPro.Api.Infrastructure.Data;

public interface IMongoContext
{
    IMongoCollection<Pedido> Pedidos { get; }
}

public class MongoContext : IMongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB") ?? "mongodb://localhost:27017";
        var databaseName = configuration["MongoDB:DatabaseName"] ?? "PetShopPro";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Pedido> Pedidos => _database.GetCollection<Pedido>("pedidos");
}
