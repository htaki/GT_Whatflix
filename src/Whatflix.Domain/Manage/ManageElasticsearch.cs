using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Repository;

namespace Whatflix.Domain.Manage
{
    public class ManageElasticsearch
    {
        IElasticsearchSettingsRepository _elasticsearchSettingsRepository;

        public ManageElasticsearch(IElasticsearchSettingsRepository elasticsearchSettingsRepository)
        {
            _elasticsearchSettingsRepository = elasticsearchSettingsRepository;
        }

        public async Task CreateIndexAsync()
        {
            await _elasticsearchSettingsRepository.CreateIndexAsync();
        }

        public async Task<IEnumerable<string>> GetIndicesAsync()
        {
            return await _elasticsearchSettingsRepository.GetIndicesAsync();
        }

        public async Task SetIndexAsync(string index)
        {
            await _elasticsearchSettingsRepository.SetIndexAsync(index);
        }

        public async Task DeleteIndexAsync(string index)
        {
            await _elasticsearchSettingsRepository.DeleteIndexAsync(index);
        }
    }
}
