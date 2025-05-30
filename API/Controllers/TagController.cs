using Framework.Model;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Request;
using Service.Interface;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    public class TagController : BaseController
    {
        private readonly ILogger<TagController> _logger;
        private readonly ITagService _tagService;

        public TagController(ILogger<TagController> logger
            , ITagService tagService)
        {
            _logger = logger;
            _tagService = tagService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/[Controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] BaseRequest request)
        {
            try
            {
                var response = await _tagService.GetAllAsync(request);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/[Controller]")]
        public async Task<IActionResult> AddAsync([FromBody] string tagTitle)
        {
            try
            {
                var response = await _tagService.AddAsync(UserId, tagTitle);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("api/[Controller]")]
        public async Task<IActionResult> UpdateAsync([FromBody] TagUpdateDTO request)
        {
            try
            {
                var response = await _tagService.UpdateAsync(UserId, request);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
