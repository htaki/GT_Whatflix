using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Whatflix.Infrastructure.Helpers.Mappers;
using Whatflix.Presentation.Api.Models;

namespace Whatflix.Presentation.Api.Helpers
{
    public class ControllerHelper
    {
        private const string CREDITS_PATH = "wwwroot/tmdb_5000_credits.csv";
        private const string MOVIES_PATH = "wwwroot/tmdb_5000_movies.csv";
        private const string USER_PREFERENCE_PATH = "wwwroot/user_preferences.json";
        private static List<UserPreferenceModel> _userPreferenceModels;

        public virtual IEnumerable<MovieModel> GetMovies()
        {
            var movies = new List<MovieModel>();

            try
            {
                ReadFromMoviesDataset(movies);
                ReadFromCreditsDataset(movies);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Please include tmdb_5000_movies.csv and tmdb_5000_credits.csv in wwwroot root folder.");
            }

            return movies;
        }

        public virtual IEnumerable<UserPreferenceModel> GetUserPreferences()
        {
            return ReadFromUserPreferenceJson();
        }

        public virtual UserPreferenceModel GetUserPreferencesByUserId(int userId)
        {
            return ReadFromUserPreferenceJson().FirstOrDefault(f => f.UserId == userId);
        }

        #region Private Methods

        private List<UserPreferenceModel> ReadFromUserPreferenceJson()
        {
            if (_userPreferenceModels == null)
            {
                using (StreamReader streamReader = new StreamReader(USER_PREFERENCE_PATH))
                {
                    var content = streamReader.ReadToEnd();
                    _userPreferenceModels = JsonConvert.DeserializeObject<List<UserPreferenceModel>>(content);
                }
            }

            return _userPreferenceModels;
        }

        private void ReadFromCreditsDataset(List<MovieModel> movies)
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

        private void ReadFromMoviesDataset(List<MovieModel> movies)
        {
            using (var reader = new StreamReader(MOVIES_PATH))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<MovieMapper>();
                foreach (var movieMapper in records)
                {
                    movies.Add(new MovieModel
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
                return originalLanguage;
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
