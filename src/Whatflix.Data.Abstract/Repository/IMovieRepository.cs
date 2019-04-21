using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;

namespace Whatflix.Data.Abstract.Repository
{
    public interface IMovieRepository
    {
        Task InsertMany(IEnumerable<IMovie> entities);
        Task<List<IMovie>> Search(string[] searchWords);
        Task<List<IMovie>> Search(string[] searchWords, string[] favoriteActors, string[] favoriteDirectors, string[] favoriteLanguages);
    }
}
