using AutoMapper;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whatflix.Data.Elasticsearch.Repository
{
    public class BaseElasticsearchRepository<TEntity, TDataObject> where TDataObject : class
    {
        protected IElasticClient _client;
        string _indexAlias;
        IMapper _mapper;

        public BaseElasticsearchRepository(ElasticsearchWrapper elasticsearchWrapper, IMapper mapper)
        {
            _client = elasticsearchWrapper.Client;
            _indexAlias = elasticsearchWrapper.IndexAlias;
            _mapper = mapper;
        }

        public async Task InsertMany(IEnumerable<TEntity> entities)
        {
            var dataObjects = _mapper.Map<IEnumerable<TDataObject>>(entities);
            await _client.IndexManyAsync(dataObjects);
        }
    }
}
