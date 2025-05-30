using Framework.Model;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs.Request;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class JobApplicationController : BaseController
    {
        private readonly ILogger<JobApplicationController> _logger;
        private readonly IJobApplicationService _jobApplicationService;

        public JobApplicationController(ILogger<JobApplicationController> logger
            , IJobApplicationService jobApplicationService)
        {
            _logger = logger;
            _jobApplicationService = jobApplicationService;
        }

        [HttpGet]
        [Route("api/[Controller]")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            try
            {
                var response = await _jobApplicationService.GetByIdAsync(id);
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
        public async Task<IActionResult> GetAllAsync(int? jobCategoryId, int? jobId, [FromQuery] BaseRequest request)
        {
            try
            {
                var response = await _jobApplicationService.GetAllAsync(jobCategoryId, jobId, request);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> AddAsync([FromForm] JobApplicationAddDTO request)
        {
            try
            {
                var response = await _jobApplicationService.AddAsync(request);
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
        [Route("api/[controller]")]
        public async Task<IActionResult> UpdateAsync([FromBody] int id)
        {
            try
            {
                var response = await _jobApplicationService.UpdateAsync(id);
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
