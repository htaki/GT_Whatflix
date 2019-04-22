using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.Movie;

namespace Whatflix.Data.Abstract.Repository
{
    public interface IMovieRepository
    {
        Task InsertMany(IEnumerable<IMovie> entities);
        Task<List<IMovie>> SearchAsync(string[] searchWords);
        Task<List<IMovie>> SearchAsync(string[] searchWords, List<int> movieIds);
        Task UpdatedAppeardInSearchAsync(List<int> movieIds);
        Task<IEnumerable<string>> GetRecommendationByMovieIdsAsync(List<int> movieIds);
    }
}
