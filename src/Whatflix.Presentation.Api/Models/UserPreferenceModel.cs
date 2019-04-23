using Newtonsoft.Json;
using System.Collections.Generic;

namespace Whatflix.Presentation.Api.Models
{
    public class UserPreferenceModel
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("preferred_languages")]
        public List<string> PreferredLanguages { get; set; }

        [JsonProperty("favourite_actors")]
        public List<string> FavoriteActors { get; set; }

        [JsonProperty("favourite_directors")]
        public List<string> FavoriteDirectors { get; set; }
    }
}
