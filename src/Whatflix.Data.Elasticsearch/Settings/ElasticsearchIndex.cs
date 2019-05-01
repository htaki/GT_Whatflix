using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Settings.Elasticsearch;

namespace Whatflix.Data.Elasticsearch.Settings
{
    public class ElasticsearchIndex : IElasticsearchIndex
    {
        private readonly ElasticsearchWrapper _elasticsearchWrapper;

        public ElasticsearchIndex(ElasticsearchWrapper elasticsearchWrapper)
        {
            _elasticsearchWrapper = elasticsearchWrapper;
        }

        public async Task<ICreateIndexResponse> CreateIndexAsync(string indexAlias)
        {
            string index = GenerateIndex(indexAlias);
            return await _elasticsearchWrapper.GetClient(indexAlias).CreateIndexAsync(index, AnalyzedMapping.MoviesIndexDescriptor);
        }

        public async Task<IEnumerable<string>> GetIndicesAsync(string indexAlias)
        {
            var catIndices = await _elasticsearchWrapper.GetClient(indexAlias).CatIndicesAsync();
            return catIndices.Records.Where(x => x.Index.StartsWith(indexAlias)).Select(x => x.Index);
        }

        public async Task<IBulkAliasResponse> SetIndexAsync(string index, string indexAlias)
        {
            var alias = await _elasticsearchWrapper.GetClient(index).GetAliasAsync(x => x.Index(index));

            if (alias.Indices[index].Aliases.Count != 0)
            {
                var Indices = await _elasticsearchWrapper.GetClient(index).GetIndicesPointingToAliasAsync(indexAlias);
                return await _elasticsearchWrapper.GetClient(index).AliasAsync(a => a
                    .Remove(i => i
                        .Index(Indices.FirstOrDefault())
                        .Alias(indexAlias)
                    )
                    .Add(i => i
                        .Alias(indexAlias)
                        .Index(index)
                    )
                );
            }
            else
            {
                return await _elasticsearchWrapper.GetClient(index).AliasAsync(a => a
                    .Add(i => i
                        .Alias(indexAlias)
                        .Index(index)
                    )
                );
            }
        }

        public async Task<IDeleteIndexResponse> DeleteIndexAsync(string index)
        {
            return await _elasticsearchWrapper.GetClient(index).DeleteIndexAsync(index);
        }

        private string GenerateIndex(string alias)
        {
            return String.Format("{0}-{1}", alias, DateTime.UtcNow.ToString("yyyyMMdd-hhmmss"));
        }
    }
}
