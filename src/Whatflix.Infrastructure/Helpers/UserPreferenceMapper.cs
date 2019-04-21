using Newtonsoft.Json;
using System.Collections.Generic;

namespace Whatflix.Infrastructure.Helpers
{
    public class UserPreferenceMapper
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("preferred_languages")]
        public List<string> PreferedLanguages { get; set; }

        [JsonProperty("favourite_actors")]
        public List<string> FavoriteActors { get; set; }

        [JsonProperty("favourite_directors")]
        public List<string> FavoriteDirectors { get; set; }
    }
}
