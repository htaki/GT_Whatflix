using Microsoft.Extensions.Options;
using Nest;
using System;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Elasticsearch.Settings
{
    public class ElasticsearchWrapper
    {
        private readonly string _connectionSrting;

        public ElasticsearchWrapper(IOptions<SettingsWrapper> serviceSettings)
        {
            _connectionSrting = serviceSettings.Value.Databases.Elasticsearch.ConnectionString;
        }

        public ElasticClient GetClient(string indexAlias)
        {
            var settings = new ConnectionSettings(new Uri(_connectionSrting));
            settings.DefaultIndex(indexAlias);
            settings.ThrowExceptions(alwaysThrow: true);
            settings.PrettyJson();

            return new ElasticClient(settings);
        }
    }
}
