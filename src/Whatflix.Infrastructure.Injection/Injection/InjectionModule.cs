using Microsoft.Extensions.DependencyInjection;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Mongo.Repository;
using Whatflix.Domain.Manage;

namespace Whatflix.Infrastructure.Injection
{
    public class InjectionModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ManageMovie>();
            services.AddScoped<IMovieRepository, MoviesMongoRepository>();
        }
    }
}
