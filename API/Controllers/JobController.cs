using System.Net;
using System.Net.Http.Headers;
using Framework.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Request;
using Service.Interface;

namespace API.Controllers
{
    [ApiController]
    public class JobController : BaseController
    {
        private readonly ILogger<JobController> _logger;
        private readonly IJobService _jobService;

        public JobController(ILogger<JobController> logger, IJobService jobService)
        {
            _logger = logger;
            _jobService = jobService;
        }

        [HttpGet]
        [Route("api/[Controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] BaseRequest request, int? categoryId, bool? isEnabled, bool? isAdmin)
        {
            try
            {
                var response = await _jobService.GetAllAsync(request, categoryId, isEnabled, isAdmin);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/[Controller]s/lookup")]
        public async Task<IActionResult> GetLookupAsync([FromQuery] BaseRequest request, int? categoryId)
        {
            try
            {
                var response = await _jobService.GetLookupAsync(request, categoryId);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/[Controller]")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            try
            {
                var response = await _jobService.GetByIdAsync(id);
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
        public async Task<IActionResult> AddAsync([FromBody] JobAddDTO request)
        {
            try
            {
                var response = await _jobService.AddAsync(UserId, request);
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
        public async Task<IActionResult> UpdateAsync([FromBody] JobUpdateDTO request)
        {
            try
            {
                var response = await _jobService.UpdateAsync(UserId, request);
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
        [Route("api/[Controller]/status")]
        public async Task<IActionResult> UpdateStatusAsync([FromBody] JobStatusUpdateDTO request)
        {
            try
            {
                var response = await _jobService.UpdateStatusAsync(UserId, request);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("api/[Controller]")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _jobService.DeleteAsync(UserId, id);
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
