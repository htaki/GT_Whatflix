using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Whatflix.Domain.Dto.Movie;
using Whatflix.Domain.Dto.UserPreference;
using Whatflix.Domain.Manage;
using Whatflix.Presentation.Api.Helpers;

namespace Whatflix.Presentation.Api.Controllers
{
    [Route("movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly Movie _manageMovie;
        private readonly ControllerHelper _controllerHelper;
        private readonly IMapper _mapper;

        public MoviesController(Movie manageMovie, ControllerHelper controllerHelper, IMapper mapper)
        {
            _manageMovie = manageMovie;
            _controllerHelper = controllerHelper;
            _mapper = mapper;
        }

        [HttpGet("user/{userId}/search")]
        public async Task<IActionResult> Get(int userId, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "Search text cannot be empty.");
                }

                var userPreference = _controllerHelper.GetUserPreferencesByUserId(userId);

                if (userPreference == null)
                {

                }

                var userMoviesTask = _manageMovie.SearchAsync(GetSearchWords(text), _mapper.Map<UserPreferenceDto>(userPreference));
                var moviesTask = _manageMovie.SearchAsync(GetSearchWords(text));
                var movieList = await Task.WhenAll(userMoviesTask, moviesTask);

                var movieIds = new List<int>();
                var movieResult = MapMovies(movieList, out movieIds);

                _manageMovie.UpdatedAppeardInSearchAsync(movieIds);
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
            try
            {
                return Ok(await _manageMovie.GetRecommendationsAsync(_mapper.Map<IEnumerable<UserPreferenceDto>>(_controllerHelper.GetUserPreferences())));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private IEnumerable<string> MapMovies(List<MovieDto>[] movieList, out List<int> movieIds)
        {
            movieIds = new List<int>();

            var userPreferredMovies = movieList[0] ?? new List<MovieDto>();
            var otherMovies = movieList[1] ?? new List<MovieDto>();
            otherMovies = otherMovies.Where(o => !userPreferredMovies.Any(u => o.Title == u.Title)).ToList();

            var movies = userPreferredMovies.Concat(otherMovies);
            movieIds = userPreferredMovies.Select(s => s.MovieId).ToList();

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