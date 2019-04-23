using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;

namespace Whatflix.Data.Abstract.Repository
{
    public interface IMovieRepository
    {
        Task InsertMany(IEnumerable<IMovieEntity> entities);
        Task<List<IMovieEntity>> SearchAsync(string[] searchWords);
        Task<List<IMovieEntity>> SearchAsync(string[] searchWords, List<string> favoriteActors, List<string> favoriteDirectors, List<string> favoriteLanguages);
        Task UpdatedAppeardInSearchAsync(List<int> movieIds);
        //Task<IEnumerable<string>> GetRecommendationByMovieIdsAsync(List<int> movieIds);
    }
}
