using Microsoft.Extensions.Options;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Elasticsearch.Test.Repository
{
    public class BaseRepositoryTest
    {
        protected IOptions<SettingsWrapper> CreateSettings()
        {
            return Options.Create(new SettingsWrapper
            {
                Databases = new Databases
                {
                    Elasticsearch = new Database
                    {
                        ConnectionString = "https://vsq4ajXp6Y:NasvTBMfGWreE4g9Ut3XA75@sandbox-cluster-9944218666.ap-southeast-2.bonsaisearch.net"
                    },
                    MongoDb = new Database
                    {
                        ConnectionString = "mongodb://admin:admin@sandbox-cluster-shard-00-00-a38l5.mongodb.net:27017,sandbox-cluster-shard-00-01-a38l5.mongodb.net:27017,sandbox-cluster-shard-00-02-a38l5.mongodb.net:27017/whatflix?ssl=true&replicaSet=sandbox-cluster-shard-0&authSource=admin&retryWrites=true"
                    }
                }
            });
        }
    }
}
