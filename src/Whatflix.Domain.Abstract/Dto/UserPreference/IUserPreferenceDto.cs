using System.Collections.Generic;

namespace Whatflix.Domain.Abstract.Dto.UserPreference
{
    public interface IUserPreferenceDto
    {
        int UserId { get; set; }
        List<string> PreferredLanguages { get; set; }
        List<string> FavoriteActors { get; set; }
        List<string> FavoriteDirectors { get; set; }
    }
}
