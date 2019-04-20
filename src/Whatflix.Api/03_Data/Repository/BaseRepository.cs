using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whatflix.Api._03_Data.Repository
{
    public class BaseRepository<T> where T : class
    {
        private IMongoClient _client;
        private IMongoDatabase _database;
        protected readonly IMongoCollection<T> _collection;
        protected readonly Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);

        public BaseRepository(string connectionString, string collectionName)
        {
            _client = new MongoClient(connectionString);
            _database = _client.GetDatabase("whatflix");
            _collection = _database.GetCollection<T>(collectionName);
        }

        public async Task InsertMany(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(entities);
        }
    }
}
