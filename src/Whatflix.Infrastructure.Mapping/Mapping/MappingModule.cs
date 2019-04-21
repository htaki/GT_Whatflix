using Microsoft.Extensions.DependencyInjection;
using Whatflix.Data.Abstract.Entities.Movie;
using Whatflix.Domain.Dto.Movie;

namespace Whatflix.Infrastructure.Mapping
{
    public class MappingModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = new AutoMapper.MapperConfiguration(c =>
            {
                /* source -> destination */
                c.CreateMap<MovieDto, IMovie>();
            });

            services.AddSingleton(configuration.CreateMapper());
        }
    }
}
