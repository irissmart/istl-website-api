using Framework.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Request;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class ClientController : BaseController
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IClientService _clientService;

        public ClientController(ILogger<ClientController> logger, IClientService clientService)
        {
            _logger = logger;
            _clientService = clientService;
        }

        [HttpGet]
        [Route("api/[Controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] BaseRequest request)
        {
            try
            {
                var response = await _clientService.GetAllAsync(request);
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
                var response = await _clientService.GetByIdAsync(id);
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
        public async Task<IActionResult> AddAsync(ClientAddDTO request)
        {
            try
            {
                var response = await _clientService.AddAsync(UserId, request);
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
        public async Task<IActionResult> UpdateAsync(ClientUpdateDTO request)
        {
            try
            {
                var response = await _clientService.UpdateAsync(UserId, request);
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
                var response = await _clientService.DeleteAsync(UserId, id);
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
