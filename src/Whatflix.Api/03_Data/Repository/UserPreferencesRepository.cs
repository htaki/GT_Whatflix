using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;
using Whatflix.Api._03_Data.Mdo.UserPreference;

namespace Whatflix.Api._03_Data.Repository
{
    public class UserPreferencesRepository : BaseRepository<UserPreferenceMdo>
    {
        public const string COLLECTION_NAME = "user-preferences";

        public UserPreferencesRepository(IOptions<ServiceSettings> serviceSettings)
            : base(serviceSettings.Value.Whatfix_Db.ConnectionString, COLLECTION_NAME)
        {
        }

        public async Task<UserPreferenceMdo> GetByUserId(int userId)
        {
            var filter = new FilterDefinitionBuilder<UserPreferenceMdo>().Eq(u => u.UserId, userId);
            var userCursor = await _collection.FindAsync(filter);
            return await userCursor.FirstOrDefaultAsync();
        }
    }
}
