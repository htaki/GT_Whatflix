using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.UserPreference;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Mongo.Mdo.UserPreference;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Mongo.Repository
{
    public class UserPreferencesMongoRepository : BaseMongoRepository<IUserPreference, UserPreferenceMdo>, IUserPreferenceRepository
    {
        public const string COLLECTION_NAME = "user-preferences";
        private IMapper _mapper;

        public UserPreferencesMongoRepository(IOptions<SettingsWrapper> serviceSettings, IMapper mapper) : base(serviceSettings, mapper, COLLECTION_NAME)
        {
            _mapper = mapper;
        }

        public async Task<List<int>> GetMovieIdsByUserIdAsync(int userId)
        {
            var userFilter = Builders<UserPreferenceMdo>.Filter.Eq(f => f.UserId, userId);
            var userPreferenceCursor = await _collection.FindAsync(userFilter);
            var userPreference = await userPreferenceCursor.FirstOrDefaultAsync();

            return userPreference?.MovieIds;
        }
    }
}
