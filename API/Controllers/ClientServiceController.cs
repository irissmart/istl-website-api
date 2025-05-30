using Framework.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class ClientServiceController : BaseController
    {
        private readonly ILogger<ClientServiceController> _logger;
        private readonly IClientServiceService _clientServiceService;

        public ClientServiceController(ILogger<ClientServiceController> logger
            , IClientServiceService clientServiceService)
        {
            _logger = logger;
            _clientServiceService = clientServiceService;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            try
            {
                var userResponse = await _clientServiceService.GetByIdAsync(id);
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
        public async Task<IActionResult> GetAllAsync(int? categoryId, [FromQuery] BaseRequest request)
        {
            try
            {
                var userResponse = await _clientServiceService.GetAllFilteredAsync(categoryId, request);
                return GenerateResponse(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/[controller]s/related")]
        public async Task<IActionResult> GetAllRelatedAsync(int id, string serviceName, [FromQuery] BaseRequest request)
        {
            try
            {
                var userResponse = await _clientServiceService.GetAllRelatedAsync(id, serviceName, request);
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
        public async Task<IActionResult> AddAsync([FromForm] ClientServiceAddDTO request)
        {
            try
            {
                var userResponse = await _clientServiceService.AddAsync(UserId, request);
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
        public async Task<IActionResult> UpdateAsync([FromForm] ClientServiceUpdateDTO request)
        {
            try
            {
                var userResponse = await _clientServiceService.UpdateAsync(UserId, request);
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
                var response = await _clientServiceService.DeleteAsync(UserId, id);
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
