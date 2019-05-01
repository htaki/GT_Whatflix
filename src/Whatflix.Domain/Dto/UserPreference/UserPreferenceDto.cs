using System.Collections.Generic;
using Whatflix.Domain.Abstract.Dto.UserPreference;

namespace Whatflix.Domain.Dto.UserPreference
{
    public class UserPreferenceDto : IUserPreferenceDto
    {
        public int UserId { get; set; }
        public List<string> PreferredLanguages { get; set; }
        public List<string> FavoriteActors { get; set; }
        public List<string> FavoriteDirectors { get; set; }
    }
}
