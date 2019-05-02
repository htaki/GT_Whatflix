using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Whatflix.Domain.Abstract.Dto.Movie;
using Whatflix.Domain.Abstract.Dto.UserPreference;
using Whatflix.Domain.Abstract.Manage;
using Whatflix.Domain.Dto.UserPreference;
using Whatflix.Presentation.Api.Helpers;

namespace Whatflix.Presentation.Api.Controllers
{
    [Route("movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ControllerHelper _controllerHelper;
        private readonly IMovie _manageMovie;
        private readonly IMapper _mapper;

        public MoviesController(ControllerHelper controllerHelper,
            IMovie manageMovie,
            IMapper mapper)
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
                if (userId <= 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "The userId is not valid.");
                }

                if (string.IsNullOrEmpty(text))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, "Search text cannot be empty.");
                }

                var userPreference = _controllerHelper.GetUserPreferencesByUserId(userId);

                if (userPreference == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, $"The user with userId: '{userId}' is not defined.");
                }

                var userMoviesTask = _manageMovie.SearchAsync(GetSearchWords(text), _mapper.Map<UserPreferenceDto>(userPreference));
                var moviesTask = _manageMovie.SearchAsync(GetSearchWords(text));
                var movieList = await Task.WhenAll(userMoviesTask, moviesTask);

                var movieIds = new List<int>();
                var movieResult = MapMovies(movieList, out movieIds);
                await _manageMovie.UpdateAppeardInSearchAsync(movieIds);

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
                return Ok(await _manageMovie.GetRecommendationsAsync(_mapper.Map<IEnumerable<IUserPreferenceDto>>(_controllerHelper.GetUserPreferences())));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private IEnumerable<string> MapMovies(List<IMovieDto>[] movieList, out List<int> movieIds)
        {
            movieIds = new List<int>();

            var userPreferredMovies = movieList[0] ?? new List<IMovieDto>();
            var otherMovies = movieList[1] ?? new List<IMovieDto>();
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