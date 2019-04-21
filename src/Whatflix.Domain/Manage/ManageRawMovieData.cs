using CsvHelper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Whatflix.Domain.Dto.Movie;
using Whatflix.Infrastructure.Helpers;

namespace Whatflix.Domain.Manage
{
    public class ManageRawMovieData
    {
        private const string CREDITS_PATH = "wwwroot/tmdb_5000_credits.csv";
        private const string MOVIES_PATH = "wwwroot/tmdb_5000_movies.csv";

        public IEnumerable<MovieDto> ReadRawData()
        {
            var movies = new List<MovieDto>();

            ReadFromMoviesDataset(movies);
            ReadFromCreditsDataset(movies);

            return movies;
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
    }
}
