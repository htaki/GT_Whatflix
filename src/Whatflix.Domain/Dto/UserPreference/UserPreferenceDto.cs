using System.Collections.Generic;

namespace Whatflix.Domain.Dto.UserPreference
{
    public class UserPreferenceDto
    {
        public int UserId { get; set; }
        public List<string> PreferredLanguages { get; set; }
        public List<string> FavoriteActors { get; set; }
        public List<string> FavoriteDirectors { get; set; }
    }
}
