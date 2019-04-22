using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.UserPreference;

namespace Whatflix.Data.Abstract.Repository
{
    public interface IUserPreferenceRepository
    {
        Task InsertMany(IEnumerable<IUserPreferenceEntity> entities);
        Task<List<int>> GetMovieIdsByUserIdAsync(int userId);
        Task<IEnumerable<IUserPreferenceEntity>> GetUserPreferences();
    }
}
