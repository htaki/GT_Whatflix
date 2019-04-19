using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whatflix.Api._02_Domain.Manage;
using Whatflix.Api._03_Data.Mdo;
using Whatflix.Api._03_Data.Repository;
using Whatflix.Api._04_Infrastructure.Helpers;

namespace Whatflix.Api._01_Presentation.Controllers
{
    [Route("database/replicate")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        ManageRawMovieData _manageRawMovieData;
        MoviesRepository _moviesRepository;
        UserPreferencesRepository _userPreferencesRepository;

        public DatabaseController(MoviesRepository moviesRepository,
          UserPreferencesRepository userPreferencesRepository)
        {
            _manageRawMovieData = new ManageRawMovieData();
            _moviesRepository = moviesRepository;
            _userPreferencesRepository = userPreferencesRepository;
        }

        [HttpPut("users")]
        public async Task<IActionResult> ReplicateUsers()
        {
            var movies = _manageRawMovieData.ReadRawData();
            var userPreferencesFromData = _manageRawMovieData.GetUserPreferenceFromData();
            var userPreferences = GetUserPreferences(movies, userPreferencesFromData);
            await _userPreferencesRepository.InsertMany(userPreferences);
            return Ok();
        }

        [HttpPut("movies")]
        public async Task<IActionResult> ReplicateMovies()
        {
            var movies = _manageRawMovieData.ReadRawData();
            await _moviesRepository.InsertMany(movies);

            return Ok();
        }

        private IEnumerable<UserPreferenceMdo> GetUserPreferences(IEnumerable<MovieMdo> movies, IEnumerable<UserPreferenceMapper> userPreferencesFromData)
        {
            var userPreferences = new List<UserPreferenceMdo>();

            foreach (var preference in userPreferencesFromData)
            {
                userPreferences.Add(new UserPreferenceMdo
                {
                    UserId = preference.UserId,
                    Movies = GetMoviesByPreference(movies, preference)
                });
            }

            return userPreferences;
        }

        private List<MovieMdo> GetMoviesByPreference(IEnumerable<MovieMdo> movies, UserPreferenceMapper preference)
        {
            var userMovies = new List<MovieMdo>();

            foreach (var movie in movies)
            {
                var actors = movie.Actors.Where(a => preference.FavoriteActors.Any(fa => fa == a)).ToList();
                var director = preference.FavoriteDirectors.FirstOrDefault(fd => fd == movie.Director);
                var language = preference.PreferedLanguages.FirstOrDefault(pl => pl == movie.Language);

                userMovies.Add(new MovieMdo
                {
                    Actors = actors,
                    Language = language,
                    Director = director,
                    MovieId = movie.MovieId,
                    Title = movie.Title
                });
            }

            return userMovies;
        }
    }
}