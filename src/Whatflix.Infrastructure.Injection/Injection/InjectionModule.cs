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
            services.AddScoped<ManageMovie>();
            services.AddScoped<ManageUserPreference>();
            services.AddScoped<ElasticsearchWrapper>();
            services.AddScoped<ManageElasticsearch>();
            services.AddScoped<IElasticsearchIndex, ElasticsearchIndex>();
        }

        public void ConfigureRepositories(IServiceCollection services, bool shouldUseMongoRepository)
        {
            if (shouldUseMongoRepository)
            {
                services.AddScoped<IMovieRepository, MoviesMongoRepository>();
                services.AddScoped<IUserPreferenceRepository, UserPreferencesMongoRepository>();
                return;
            }

            services.AddTransient<IMovieRepository, MoviesElasticsearchRepository>();
            services.AddTransient<IUserPreferenceRepository, UserPreferencesElasticsearchRepository>();
        }
    }
}
