using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Whatflix.Domain.Abstract.Manage;
using Whatflix.Domain.Dto.Movie;
using Whatflix.Presentation.Api.Helpers;

namespace Whatflix.Presentation.Api.Controllers
{
    [Route("documents")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly ControllerHelper _controllerHelper;
        private readonly IMovie _manageMovie;
        private readonly IMapper _mapper;

        public DocumentsController(ControllerHelper controllerHelper, 
            IMovie manageMovie,
            IMapper mapper)
        {
            _controllerHelper = controllerHelper;
            _manageMovie = manageMovie;
            _mapper = mapper;
        }

        [HttpPut("replicate/movies")]
        public async Task<IActionResult> ReplicateMovies()
        {
            try
            {
                var movies = _controllerHelper.GetMovies();
                await _manageMovie.InsertMany(_mapper.Map<IEnumerable<MovieDto>>(movies));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
