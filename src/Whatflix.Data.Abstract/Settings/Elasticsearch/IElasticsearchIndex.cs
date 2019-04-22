using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whatflix.Data.Abstract.Settings.Elasticsearch
{
    public interface IElasticsearchIndex
    {
        Task<ICreateIndexResponse> CreateIndexAsync(string indexAlias);
        Task<IEnumerable<string>> GetIndicesAsync(string indexAlias);
        Task<IBulkAliasResponse> SetIndexAsync(string index, string indexAlias);
        Task<IDeleteIndexResponse> DeleteIndexAsync(string index);
    }
}
