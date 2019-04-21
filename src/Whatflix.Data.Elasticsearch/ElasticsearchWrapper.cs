using Microsoft.Extensions.Options;
using Nest;
using System;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Elasticsearch
{
    public class ElasticsearchWrapper
    {
        private readonly string _connectionSrting;
        private const string INDEX_ALIAS = "whatflix";

        public ElasticsearchWrapper(IOptions<SettingsWrapper> serviceSettings)
        {
            _connectionSrting = serviceSettings.Value.Databases.Elasticsearch.ConnectionString;
        }

        public string IndexAlias
        {
            get
            {
                return INDEX_ALIAS;
            }
        }

        public ElasticClient Client
        {
            get
            {
                var settings = new ConnectionSettings(new Uri(_connectionSrting));
                settings.DefaultIndex(IndexAlias);
                settings.ThrowExceptions(alwaysThrow: true);
                settings.PrettyJson();

                return new ElasticClient(settings);
            }
        }
    }
}
