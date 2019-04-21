using MongoDB.Driver;

namespace Whatflix.Data.Mongo.Repository
{
    public class BaseMongoRepository<T>
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        protected readonly IMongoCollection<T> _collection;
        protected readonly Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);

        public BaseMongoRepository(string connectionString, string collectionName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("whatflix");
            _collection = _database.GetCollection<T>(collectionName);
        }
    }
}
