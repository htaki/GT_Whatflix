using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;

namespace Whatflix.Data.Abstract.Repository
{
    public interface IMovieRepository
    {
        Task InsertMany(IEnumerable<IMovieEntity> entities);
        Task<List<IMovieEntity>> SearchAsync(string[] searchWords);
        Task<List<IMovieEntity>> SearchAsync(string[] searchWords, List<string> favoriteActors, List<string> favoriteDirectors, List<string> preferredLanguages);
        Task UpdateAppeardInSearchAsync(List<int> movieIds);
        Task<List<IMovieEntity>> GetRecommendationsAsync(List<string> favoriteActors, List<string> favoriteDirectors, List<string> preferredLanguages);
    }
}
