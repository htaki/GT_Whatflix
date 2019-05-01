using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Domain.Abstract.Dto.Movie;
using Whatflix.Domain.Abstract.Dto.UserPreference;

namespace Whatflix.Domain.Abstract.Manage
{
    public interface IMovie
    {
        Task InsertMany(IEnumerable<IMovieDto> movieDtos);
        Task<List<IMovieDto>> SearchAsync(string[] searchWords, IUserPreferenceDto userPreference);
        void UpdateAppeardInSearchAsync(List<int> movieIds);
        Task<List<IMovieDto>> SearchAsync(string[] searchWords);
        Task<List<IRecommendationsDto>> GetRecommendationsAsync(IEnumerable<IUserPreferenceDto> userPreferences);
    }
}
