using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Whatflix.Api._03_Data.Mdo.Movie;
using Whatflix.Api._03_Data.Repository;

namespace Whatflix.Api._01_Presentation.Controllers
{
    [Route("movies")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        MoviesRepository _moviesRepository;
        UserPreferencesRepository _userPreferencesRepository;

        public UsersController(MoviesRepository moviesRepository,
          UserPreferencesRepository userPreferencesRepository)
        {
            _moviesRepository = moviesRepository;
            _userPreferencesRepository = userPreferencesRepository;
        }

        [HttpGet("user/{userId}/search")]
        public async Task<IActionResult> Get(int userId, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "Searchtext cannot be empty.");
                }

                var searchWords = text.Split(',').Select(s => s.Trim()).ToArray();
                var userPreference = await _userPreferencesRepository.GetByUserId(userId);

                if (userPreference == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, $"Could not find user preferences for for user with Id: {userId}");
                }

                var userMoviesTask = _moviesRepository.Search(searchWords, userPreference);
                var moviesTask = _moviesRepository.Search(searchWords);
                var movieList = await Task.WhenAll(userMoviesTask, moviesTask);

                return Ok(MapMovies(movieList));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("users")]
        public IActionResult Get()
        {
            return Ok();
        }

        private object MapMovies(List<MovieMdo>[] movieList)
        {
            var userPreferedMovies = movieList[0];
            var otherMovies = movieList[1];
            otherMovies = otherMovies.Where(o => !userPreferedMovies.Any(u => o.Title == u.Title)).ToList();

            return userPreferedMovies.Select(m => m.Title).Concat(otherMovies.Select(m => m.Title));
        }
    }
}