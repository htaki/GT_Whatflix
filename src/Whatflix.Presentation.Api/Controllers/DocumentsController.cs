using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Whatflix.Domain.Manage;

namespace Whatflix.Presentation.Api.Controllers
{
    [Route("documents/replicate")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        ManageRawMovieData _manageRawMovieData;
        ManageMovie _manageMovie;

        public DocumentsController(ManageMovie moviesRepository)
        {
            _manageRawMovieData = new ManageRawMovieData();
            _manageMovie = moviesRepository;
        }

        [HttpPut("movies")]
        public async Task<IActionResult> ReplicateMovies()
        {
            try
            {
                var movies = _manageRawMovieData.ReadRawData();
                await _manageMovie.InsertMany(movies);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
