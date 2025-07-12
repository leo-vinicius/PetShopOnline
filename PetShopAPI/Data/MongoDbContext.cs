using MongoDB.Driver;
using PetShopAPI.Models.MongoDB;

namespace PetShopAPI.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDbConnection");
            var databaseName = configuration.GetConnectionString("MongoDbDatabase");
            
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<AdministradorMongo> Administradores =>
            _database.GetCollection<AdministradorMongo>("administradores");

        public IMongoCollection<ClienteMongo> Clientes =>
            _database.GetCollection<ClienteMongo>("clientes");

        public IMongoCollection<SessionMongo> Sessions =>
            _database.GetCollection<SessionMongo>("sessions");
    }
}
