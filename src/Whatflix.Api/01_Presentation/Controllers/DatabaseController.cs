using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Whatflix.Api._02_Domain.Manage;
using Whatflix.Api._03_Data.Mdo.Movie;
using Whatflix.Api._03_Data.Mdo.UserPreference;
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
                    FavoriteActors = preference.FavoriteActors,
                    FavoriteDirectors = preference.FavoriteDirectors,
                    PreferedLanguages = preference.PreferedLanguages
                });
            }

            return userPreferences;
        }
    }
}