using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Whatflix.Domain.Dto.Movie;
using Whatflix.Domain.Dto.UserPreference;
using Whatflix.Infrastructure.Helpers;
using Whatflix.Infrastructure.Helpers.Mappers;

namespace Whatflix.Domain.Manage
{
    public class ManageRawData
    {
        private const string CREDITS_PATH = "wwwroot/tmdb_5000_credits.csv";
        private const string MOVIES_PATH = "wwwroot/tmdb_5000_movies.csv";
        private const string USER_PREFERENCE_PATH = "wwwroot/user_preferences.json";

        public IEnumerable<MovieDto> GetMovies()
        {
            var movies = new List<MovieDto>();

            ReadFromMoviesDataset(movies);
            ReadFromCreditsDataset(movies);

            return movies;
        }

        public IEnumerable<UserPreferenceDto> GetUserPreferences()
        {
            return ReadFromUserPreferenceJson();
        }

        #region Private Methods

        private List<UserPreferenceDto> ReadFromUserPreferenceJson()
        {
            using (StreamReader streamReader = new StreamReader(USER_PREFERENCE_PATH))
            {
                var content = streamReader.ReadToEnd();
                var userPreferenceMappers = JsonConvert.DeserializeObject<List<UserPreferenceMapper>>(content);
                var movies = GetMovies();
                var userPreferences = new List<UserPreferenceDto>();

                foreach (var userPreferenceMapper in userPreferenceMappers)
                {
                    var userMovies = movies.Where(m => userPreferenceMapper.PreferredLanguages.Any(l => m.Language == l));
                    userMovies = userMovies.Where(m => userPreferenceMapper.FavoriteDirectors.Any(d => m.Director == d) ||
                        userPreferenceMapper.FavoriteActors.Any(fa => m.Actors.Any(a => a == fa))
                    );

                    var userPreference = new UserPreferenceDto
                    {
                        MovieIds = userMovies.Select(m => m.MovieId).ToList(),
                        UserId = userPreferenceMapper.UserId
                    };

                    userPreferences.Add(userPreference);
                }

                return userPreferences;
            }
        }

        private void ReadFromCreditsDataset(List<MovieDto> movies)
        {
            using (var reader = new StreamReader(CREDITS_PATH))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<CreditsMapper>();

                foreach (var movieMapper in records)
                {
                    var movieId = movieMapper.MovieId;
                    var movie = movies.FirstOrDefault(m => m.MovieId == movieId);
                    movie.Actors = GetCast(movieMapper.Cast);
                    movie.Director = GetDirector(movieMapper.Crew);
                    movie.Title = movieMapper.Title;
                }
            }
        }

        private void ReadFromMoviesDataset(List<MovieDto> movies)
        {
            using (var reader = new StreamReader(MOVIES_PATH))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<MovieMapper>();
                foreach (var movieMapper in records)
                {
                    movies.Add(new MovieDto
                    {
                        MovieId = movieMapper.Id,
                        Language = GetLanguageFromCulture(movieMapper.OriginalLanguage)
                    });
                }
            }
        }

        private string GetLanguageFromCulture(string originalLanguage)
        {
            try
            {
                return CultureInfo.GetCultureInfo(originalLanguage).EnglishName;
            }
            catch
            {
                return "English";
            }
        }

        private string GetDirector(string data)
        {
            var arr = JArray.Parse(data);
            var director = "";

            foreach (var token in arr)
            {
                var tokenValue = GetTokenValue(token, "job");

                if (tokenValue == "Director")
                {
                    director = GetTokenValue(token, "name");
                }
            }

            return director;
        }

        private List<string> GetCast(string data)
        {
            var arr = JArray.Parse(data);
            var cast = new List<string>();

            foreach (var token in arr)
            {
                cast.Add(GetTokenValue(token, "name"));
            }

            return cast;
        }

        private string GetTokenValue(JToken token, string name)
        {
            var itemProperties = token.Children<JProperty>();
            var myElement = itemProperties.FirstOrDefault(x => x.Name == name);
            var myElementValue = myElement.Value;

            return Convert.ToString(myElementValue);
        }

        #endregion
    }
}
