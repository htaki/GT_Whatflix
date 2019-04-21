namespace Whatflix.Domain.Dto.UserPreference
{
    public class UserPreferenceDto
    {
        public int UserId { get; set; }
        public string[] PreferedLanguages { get; set; }
        public string[] FavoriteActors { get; set; }
        public string[] FavoriteDirectors { get; set; }
    }
}
