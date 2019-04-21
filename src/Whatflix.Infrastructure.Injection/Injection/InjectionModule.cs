﻿using Microsoft.Extensions.DependencyInjection;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Elasticsearch;
using Whatflix.Data.Elasticsearch.Repository;
using Whatflix.Data.Mongo.Repository;
using Whatflix.Domain.Manage;

namespace Whatflix.Infrastructure.Injection
{
    public class InjectionModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ManageMovie>();
            services.AddScoped<ManageElasticsearch>();
            services.AddScoped<ElasticsearchWrapper>();
            services.AddScoped<IElasticsearchSettingsRepository, ElasticsearchSettingsRepository>();
        }

        public void ConfigureRepositories(IServiceCollection services, bool shouldUseMongoRepository)
        {
            if (shouldUseMongoRepository)
            {
                services.AddScoped<IMovieRepository, MoviesMongoRepository>();
                return;
            }

            services.AddScoped<IMovieRepository, MoviesElasticsearchRepository>();
        }
    }
}
