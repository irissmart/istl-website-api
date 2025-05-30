using Framework.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Request;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class TestimonialController : BaseController
    {
        private readonly ILogger<TestimonialController> _logger;
        private readonly ITestimonialService _testimonialService;

        public TestimonialController(ILogger<TestimonialController> logger
            , ITestimonialService testimonialService)
        {
            _logger = logger;
            _testimonialService = testimonialService;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            try
            {
                var response = await _testimonialService.GetByIdAsync(id);
                return GenerateResponse(response);
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
                var response = await _testimonialService.GetAllAsync(request);
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
        [Route("api/[controller]")]
        public async Task<IActionResult> AddAsync([FromForm] TestimonialAddDTO request)
        {
            try
            {
                var response = await _testimonialService.AddAsync(UserId, request);
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
        public async Task<IActionResult> UpdateAsync([FromForm] TestimonialUpdateDTO request)
        {
            try
            {
                var response = await _testimonialService.UpdateAsync(UserId, request);
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
        [Route("api/[controller]")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _testimonialService.DeleteAsync(UserId, id);
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
