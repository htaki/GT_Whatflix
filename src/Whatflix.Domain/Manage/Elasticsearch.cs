using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Settings.Elasticsearch;

namespace Whatflix.Domain.Manage
{
    public class Elasticsearch
    {
        IElasticsearchIndex _elasticsearchIndex;

        public Elasticsearch(IElasticsearchIndex elasticsearchIndex)
        {
            _elasticsearchIndex = elasticsearchIndex;
        }

        public async Task CreateIndexAsync(string indexAlias)
        {
            await _elasticsearchIndex.CreateIndexAsync(indexAlias);
        }

        public async Task<IEnumerable<string>> GetIndicesAsync(string indexAlias)
        {
            return await _elasticsearchIndex.GetIndicesAsync(indexAlias);
        }

        public async Task SetIndexAsync(string index, string indexAlias)
        {
            await _elasticsearchIndex.SetIndexAsync(index, indexAlias);
        }

        public async Task DeleteIndexAsync(string index)
        {
            await _elasticsearchIndex.DeleteIndexAsync(index);
        }
    }
}
