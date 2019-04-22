using AutoMapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatflix.Data.Abstract.Entities.UserPreference;
using Whatflix.Data.Abstract.Repository;
using Whatflix.Data.Mongo.Ado.UserPreference;
using Whatflix.Infrastructure.ServiceSettings;

namespace Whatflix.Data.Elasticsearch.Repository
{
    public class UserPreferencesElasticsearchRepository : BaseElasticsearchRepository<IUserPreferenceEntity, UserPreferenceAdo>, IUserPreferenceRepository
    {
        private readonly IMapper _mapper;
        private const string INDEX_ALIAS_USER_PREFERENCES = "whatflix-user-preference";

        public UserPreferencesElasticsearchRepository(IOptions<SettingsWrapper> serviceSettings, IMapper mapper) : base(serviceSettings, INDEX_ALIAS_USER_PREFERENCES, mapper)
        {
            _mapper = mapper;
        }

        public async Task<List<int>> GetMovieIdsByUserIdAsync(int userId)
        {
            var searchResponse = await _client.SearchAsync<UserPreferenceAdo>(s => s
                .Query(query => query
                    .Term(t => t
                        .Field(f => f.UserId)
                        .Value(userId)
                    )
                )
                .Size(1)
            );

            var doucment = searchResponse.Documents?.FirstOrDefault();
            return doucment.MovieIds;
        }
    }
}
