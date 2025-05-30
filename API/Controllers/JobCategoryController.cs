using Framework.Model;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Request;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class JobCategoryController : BaseController
    {
        private readonly ILogger<JobCategory> _logger;
        private readonly IJobCategoryService _jobCategoryService;

        public JobCategoryController(ILogger<JobCategory> logger
            , IJobCategoryService jobCategoryService)
        {
            _logger = logger;
            _jobCategoryService = jobCategoryService;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            try
            {
                var userResponse = await _jobCategoryService.GetByIdAsync(id);
                return GenerateResponse(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/[controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] BaseRequest request)
        {
            try
            {
                var userResponse = await _jobCategoryService.GetAllAsync(request);
                return GenerateResponse(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> AddAsync([FromForm] JobCategoryAddDTO request)
        {
            try
            {
                var userResponse = await _jobCategoryService.AddAsync(UserId, request);
                return GenerateResponse(userResponse);
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
        public async Task<IActionResult> UpdateAsync([FromForm] JobCategoryUpdateDTO request)
        {
            try
            {
                var userResponse = await _jobCategoryService.UpdateAsync(UserId, request);
                return GenerateResponse(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("api/[controller]")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var userResponse = await _jobCategoryService.DeleteAsync(UserId, id);
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
