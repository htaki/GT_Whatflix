using Microsoft.Extensions.Options;
using Whatflix.Api._03_Data.Mdo;

namespace Whatflix.Api._03_Data.Repository
{
    public class UserPreferencesRepository : BaseRepository<UserPreferenceMdo>
    {
        public const string COLLECTION_NAME = "user-preferences";

        public UserPreferencesRepository(IOptions<ServiceSettings> serviceSettings)
            : base(serviceSettings.Value.Whatfix_Db.ConnectionString, COLLECTION_NAME)
        {
        }
    }
}
