using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Whatflix.Domain.Manage;

namespace Whatflix.Presentation.Api.Controllers
{
    [Route("documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly ManageRawData _manageRawData;
        private readonly ManageMovie _manageMovie;
        private readonly ManageUserPreference _manageUserPreference;

        public DocumentsController(ManageMovie moviesRepository, ManageUserPreference manageUserPreference)
        {
            _manageRawData = new ManageRawData();
            _manageMovie = moviesRepository;
            _manageUserPreference = manageUserPreference;
        }

        [HttpPut("replicate/movies")]
        public async Task<IActionResult> ReplicateMovies()
        {
            try
            {
                var movies = _manageRawData.GetMovies();
                await _manageMovie.InsertMany(movies);
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
                var userPreferences = _manageRawData.GetUserPreferences();
                await _manageUserPreference.InsertMany(userPreferences);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
