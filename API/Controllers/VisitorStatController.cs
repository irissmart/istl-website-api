using Azure.Core;
using Framework.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs.Request;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class VisitorStatController : BaseController
    {
        private readonly ILogger<JobController> _logger;
        private readonly IVisitorStatsService _visitorStatsService;

        public VisitorStatController(ILogger<JobController> logger, IVisitorStatsService visitorStatsService)
        {
            _logger = logger;
            _visitorStatsService = visitorStatsService;
        }

        [Authorize]
        [HttpPost]
        [Route("api/[Controller]")]
        public async Task<IActionResult> AddAsync([FromBody] VisitLogDto dto)
        {
            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

                var response = await _visitorStatsService.AddAsync(dto, ip);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/[Controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var response = await _visitorStatsService.GetAllAsync(startDate, endDate);
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
