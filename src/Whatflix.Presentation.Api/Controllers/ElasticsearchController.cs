using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Whatflix.Domain.Manage;

namespace Whatflix.Presentation.Api.Controllers
{
    [Route("elasticsearch")]
    [ApiController]
    public class ElasticsearchController : ControllerBase
    {
        private readonly ManageElasticsearch _manageElasticsearch;

        public ElasticsearchController(ManageElasticsearch manageElasticsearch)
        {
            _manageElasticsearch = manageElasticsearch;
        }

        [HttpGet("index")]
        public async Task<IActionResult> GetIndices()
        {
            try
            {
                return Ok(await _manageElasticsearch.GetIndicesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("index")]
        public async Task<IActionResult> CreateIndex()
        {
            try
            {
                await _manageElasticsearch.CreateIndexAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("index")]
        public async Task<IActionResult> SetIndex(string index)
        {
            try
            {
                await _manageElasticsearch.SetIndexAsync(index);
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