using Framework.Model;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class PageController : BaseController
    {
        private readonly ILogger<PageController> _logger;
        private readonly IPageService _pageService;

        public PageController(ILogger<PageController> logger
            , IPageService pageService)
        {
            _logger = logger;
            _pageService = pageService;
        }

        [HttpGet]
        [Route("api/[controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] BaseRequest dto)
        {
            try
            {
                var userResponse = await _pageService.GetAllAsync(dto);
                return GenerateResponse(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
