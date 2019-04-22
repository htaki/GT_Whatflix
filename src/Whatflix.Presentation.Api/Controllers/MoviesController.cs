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

                var movieIds = new List<int>();
                var movieResult = MapMovies(movieList, out movieIds);

                _manageMovie.UpdatedAppeardOnSearchAsync(movieIds);
                return Ok(movieResult);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetRecommendations()
        {
            return Ok(await _manageMovie.GetRecommendationsAsync());
        }

        private IEnumerable<string> MapMovies(List<MovieDto>[] movieList, out List<int> movieIds)
        {
            movieIds = new List<int>();

            var userPreferredMovies = movieList[0] ?? new List<MovieDto>();
            var otherMovies = movieList[1] ?? new List<MovieDto>();
            otherMovies = otherMovies.Where(o => !userPreferredMovies.Any(u => o.Title == u.Title)).ToList();

            var movies = userPreferredMovies.Concat(otherMovies);
            movieIds = movies.Select(s => s.MovieId).ToList();

            return movies.Select(s => s.Title);
        }

        private string[] GetSearchWords(string text)
        {
            return text.Split(',')
                    .Select(s => s.Trim())
                    .ToArray();
        }
    }
}