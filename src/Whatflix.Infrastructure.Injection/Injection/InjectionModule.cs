using Microsoft.Extensions.DependencyInjection;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Abstract.Settings.Elasticsearch;
using Whatflix.Data.Elasticsearch.Repository;
using Whatflix.Data.Elasticsearch.Settings;
using Whatflix.Data.Mongo.Repository;
using Whatflix.Domain.Manage;

namespace Whatflix.Infrastructure.Injection
{
    public class InjectionModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Movie>();
            services.AddScoped<ElasticsearchWrapper>();
            services.AddScoped<Domain.Manage.Elasticsearch>();
            services.AddScoped<IElasticsearchIndex, ElasticsearchIndex>();
        }

        public void ConfigureRepositories(IServiceCollection services, bool shouldUseMongoRepository)
        {
            if (shouldUseMongoRepository)
            {
                services.AddScoped<IMovieRepository, MoviesMongoRepository>();
                return;
            }

            services.AddTransient<IMovieRepository, MoviesElasticsearchRepository>();
        }
    }
}
