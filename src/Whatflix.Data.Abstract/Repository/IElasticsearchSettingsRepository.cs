using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whatflix.Data.Abstract.Repository
{
    public interface IElasticsearchSettingsRepository
    {
        Task<ICreateIndexResponse> CreateIndexAsync();
        Task<IEnumerable<string>> GetIndicesAsync();
        Task<IBulkAliasResponse> SetIndexAsync(string index);
        Task<IDeleteIndexResponse> DeleteIndexAsync(string index);
    }
}
