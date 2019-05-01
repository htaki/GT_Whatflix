using AutoMapper;
using Microsoft.Extensions.Options;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Elasticsearch.Repository
{
    public abstract class BaseElasticsearchRepository<TEntity, TDataObject> where TDataObject : class
    {
        protected readonly IElasticClient _client;
        private readonly IMapper _mapper;
        private readonly string _indexAlias;

        public BaseElasticsearchRepository(IOptions<SettingsWrapper> serviceSettings, string indexAlias, IMapper mapper)
        {
            var settings = new ConnectionSettings(new Uri(serviceSettings.Value.Databases.Elasticsearch.ConnectionString));
            settings.DefaultIndex(indexAlias);
            settings.ThrowExceptions(alwaysThrow: true);
            settings.PrettyJson();

            _client = new ElasticClient(settings);
            _mapper = mapper;
            _indexAlias = indexAlias;
        }

        public async Task InsertMany(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var dataObjects = _mapper.Map<IEnumerable<TDataObject>>(entities);
            var result = await _client.IndexManyAsync(dataObjects);
        }
    }
}
