using System.Net;
using Framework.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs.Request;
using Service.Interface;

namespace API.Controllers
{
    [ApiController]
    public class PartnerController : BaseController
    {
        private readonly ILogger<PartnerController> _logger;
        private readonly IPartnerService _partnerService;

        public PartnerController(ILogger<PartnerController> logger, IPartnerService partnerService)
        {
            _logger = logger;
            _partnerService = partnerService;
        }

        [HttpGet]
        [Route("api/[Controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] BaseRequest request)
        {
            try
            {
                var response = await _partnerService.GetAllAsync(request);
                return GenerateResponse(response);
            }
            catch(Exception ex)
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
                var response = await _partnerService.GetByIdAsync(id);
                return GenerateResponse(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/[Controller]")]
        public async Task<IActionResult> AddAsync([FromForm] PartnerAddDTO request)
        {
            try
            {
                var response = await _partnerService.AddAsync(UserId, request);
                return GenerateResponse(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        [Route("api/[Controller]")]
        public async Task<IActionResult> UpdateAsync(PartnerUpdateDTO request)
        {
            try
            {
                var response = await _partnerService.UpdateAsync(UserId, request);
                return GenerateResponse(response);
            }
            catch(Exception ex)
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
                var response = await _partnerService.DeleteAsync(UserId, id);
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
