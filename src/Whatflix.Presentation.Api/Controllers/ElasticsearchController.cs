using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Whatflix.Domain.Manage;
using Whatflix.Infrastructure.Helpers.Constants;

namespace Whatflix.Presentation.Api.Controllers
{
    [Route("elasticsearch")]
    [ApiController]
    public class ElasticsearchController : ControllerBase
    {
        private readonly Domain.Manage.Elasticsearch _manageElasticsearch;

        public ElasticsearchController(Domain.Manage.Elasticsearch manageElasticsearch)
        {
            _manageElasticsearch = manageElasticsearch;
        }

        [HttpGet("movies/index")]
        public async Task<IActionResult> GetMoviesIndices()
        {
            try
            {
                return Ok(await _manageElasticsearch.GetIndicesAsync(IndexConstant.INDEX_ALIAS_MOVIES));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("movies/index")]
        public async Task<IActionResult> CreateMoviesIndex()
        {
            try
            {
                await _manageElasticsearch.CreateIndexAsync(IndexConstant.INDEX_ALIAS_MOVIES);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("movies/index")]
        public async Task<IActionResult> SetMoviesIndex(string index)
        {
            try
            {
                await _manageElasticsearch.SetIndexAsync(index, IndexConstant.INDEX_ALIAS_MOVIES);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("index")]
        public async Task<IActionResult> DeleteIndex(string index)
        {
            try
            {
                await _manageElasticsearch.DeleteIndexAsync(index);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}