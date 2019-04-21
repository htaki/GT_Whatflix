using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Mongo.Repository
{
    public class BaseMongoRepository<TEntity, TDataObject>
    {
        private IMapper _mapper;
        private IMongoClient _client;
        private IMongoDatabase _database;
        protected readonly IMongoCollection<TDataObject> _collection;
        protected readonly Collation _caseInsensitiveCollation = new Collation("en", strength: CollationStrength.Primary);

        public BaseMongoRepository(IOptions<SettingsWrapper> serviceSettings, IMapper mapper, string collectionName)
        {
            _client = new MongoClient(serviceSettings.Value.Databases.MongoDb.ConnectionString);
            _database = _client.GetDatabase("whatflix");
            _collection = _database.GetCollection<TDataObject>(collectionName);
            _mapper = mapper;
        }

        public async Task InsertMany(IEnumerable<TEntity> entities)
        {
            var mongoDataObjects = _mapper.Map<IEnumerable<TDataObject>>(entities);
            await _collection.InsertManyAsync(mongoDataObjects);
        }
    }
}
