using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Whatflix.Domain.Dto.Movie;
using Whatflix.Domain.Manage;

namespace Whatflix.Presentation.Api.Controllers
{
    [Route("movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        ManageMovie _manageMovie;

        public MoviesController(ManageMovie manageMovie)
        {
            _manageMovie = manageMovie;
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
                var userPreference = await ManageUserPreference.Instance.GetUserPreferenceById(userId);

                if (userPreference == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, $"Could not find user preferences for for user with Id: {userId}");
                }

                var userMoviesTask = _manageMovie.Search(searchWords, userPreference);
                var moviesTask = _manageMovie.Search(searchWords);
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

        private IEnumerable<string> MapMovies(List<MovieDto>[] movieList)
        {
            var userPreferedMovies = movieList[0];
            var otherMovies = movieList[1];
            otherMovies = otherMovies.Where(o => !userPreferedMovies.Any(u => o.Title == u.Title)).ToList();

            return userPreferedMovies.Select(m => m.Title).Concat(otherMovies.Select(m => m.Title));
        }
    }
}