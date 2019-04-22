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

                var userMoviesTask = _manageMovie.SearchAsync(GetSearchWords(text), userId);
                var moviesTask = _manageMovie.SearchAsync(GetSearchWords(text));
                var movieList = await Task.WhenAll(userMoviesTask, moviesTask);

                return Ok(MapMovies(movieList));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("users")]
        public IActionResult GetRecommendations()
        {
            return Ok();
        }

        private IEnumerable<string> MapMovies(List<MovieDto>[] movieList)
        {
            var userPreferredMovies = movieList[0] ?? new List<MovieDto>();
            var otherMovies = movieList[1] ?? new List<MovieDto>();
            otherMovies = otherMovies.Where(o => !userPreferredMovies.Any(u => o.Title == u.Title)).ToList();

            return userPreferredMovies.Select(m => m.Title).Concat(otherMovies.Select(m => m.Title));
        }

        private string[] GetSearchWords(string text)
        {
            return text.Split(',')
                    .Select(s => s.Trim())
                    .ToArray();
        }
    }
}