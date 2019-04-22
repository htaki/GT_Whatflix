using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Whatflix.Domain.Dto.Movie;
using Whatflix.Domain.Dto.UserPreference;
using Whatflix.Domain.Manage;
using Whatflix.Presentation.Api.Helpers;

namespace Whatflix.Presentation.Api.Controllers
{
    [Route("documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly DocumentsControllerHelper _documentsControllerHelper;
        private readonly Movie _manageMovie;
        private readonly UserPreference _manageUserPreference;
        private readonly IMapper _mapper;

        public DocumentsController(Movie moviesRepository,
            UserPreference manageUserPreference,
            DocumentsControllerHelper documentsControllerHelper,
            IMapper mapper)
        {
            _documentsControllerHelper = documentsControllerHelper;
            _manageMovie = moviesRepository;
            _manageUserPreference = manageUserPreference;
            _mapper = mapper;
        }

        [HttpPut("replicate/movies")]
        public async Task<IActionResult> ReplicateMovies()
        {
            try
            {
                var movies = _documentsControllerHelper.GetMovies();
                await _manageMovie.InsertMany(_mapper.Map<IEnumerable<MovieDto>>(movies));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("replicate/user-preferences")]
        public async Task<IActionResult> ReplicateUserPreferences()
        {
            try
            {
                var userPreferences = _documentsControllerHelper.GetUserPreferences();
                await _manageUserPreference.InsertMany(_mapper.Map<IEnumerable<UserPreferenceDto>>(userPreferences));

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
