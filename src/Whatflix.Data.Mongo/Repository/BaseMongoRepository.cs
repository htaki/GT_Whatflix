using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Mongo.Repository
{
    public class BaseMongoRepository<T>
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        protected readonly IMongoCollection<T> _collection;
        protected readonly Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);

        public BaseMongoRepository(IOptions<SettingsWrapper> serviceSettings, string collectionName)
        {
            _client = new MongoClient(serviceSettings.Value.Databases.MongoDb.ConnectionString);
            _database = _client.GetDatabase("whatflix");
            _collection = _database.GetCollection<T>(collectionName);
        }
    }
}
