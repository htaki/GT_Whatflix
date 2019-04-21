using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Repository;

namespace Whatflix.Data.Elasticsearch.Repository
{
    public class ElasticsearchSettingsRepository : IElasticsearchSettingsRepository
    {
        ElasticsearchWrapper _elasticsearchWrapper;

        public ElasticsearchSettingsRepository(ElasticsearchWrapper elasticsearchWrapper)
        {
            _elasticsearchWrapper = elasticsearchWrapper;
        }

        public async Task<ICreateIndexResponse> CreateIndexAsync()
        {
            string index = GenerateIndex(_elasticsearchWrapper.IndexAlias);
            return await _elasticsearchWrapper.Client.CreateIndexAsync(index, AnalyzedMapping.IndexDescriptor);
        }

        public async Task<IEnumerable<string>> GetIndicesAsync()
        {
            var catIndices = await _elasticsearchWrapper.Client.CatIndicesAsync();
            return catIndices.Records.Where(x => x.Index.StartsWith(_elasticsearchWrapper.IndexAlias)).Select(x => x.Index);
        }

        public async Task<IBulkAliasResponse> SetIndexAsync(string index)
        {
            IGetAliasResponse alias = await _elasticsearchWrapper.Client.GetAliasAsync(x => x.Index(index));

            if (alias.Indices[index].Aliases.Count != 0)
            {
                IEnumerable<string> Indices = await _elasticsearchWrapper.Client.GetIndicesPointingToAliasAsync(_elasticsearchWrapper.IndexAlias);

                return await _elasticsearchWrapper.Client.AliasAsync(a => a
                    .Remove(i => i
                        .Index(Indices.FirstOrDefault())
                        .Alias(_elasticsearchWrapper.IndexAlias)
                    )
                    .Add(i => i
                        .Alias(_elasticsearchWrapper.IndexAlias)
                        .Index(index)
                    )
                );
            }
            else
            {
                return await _elasticsearchWrapper.Client.AliasAsync(a => a
                    .Add(i => i
                        .Alias(_elasticsearchWrapper.IndexAlias)
                        .Index(index)
                    )
                );
            }
        }

        public async Task<IDeleteIndexResponse> DeleteIndexAsync(string index)
        {
            return await _elasticsearchWrapper.Client.DeleteIndexAsync(index);
        }

        private string GenerateIndex(string alias)
        {
            return String.Format("{0}-{1}", alias, DateTime.UtcNow.ToString("yyyyMMdd-hhmmss"));
        }
    }
}
