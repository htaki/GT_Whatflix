using Newtonsoft.Json;

namespace Whatflix.Domain.Dto.UserPreference
{
    public class UserPreferenceDto
    {
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("preferred_languages")]
        public string[] PreferedLanguages { get; set; }

        [JsonProperty("favourite_actors")]
        public string[] FavoriteActors { get; set; }

        [JsonProperty("favourite_directors")]
        public string[] FavoriteDirectors { get; set; }
    }
}
