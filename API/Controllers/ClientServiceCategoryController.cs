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
    public class ClientServiceCategoryController : BaseController
    {
        private readonly ILogger<ClientServiceCategoryController> _logger;
        private readonly IClientServiceCategoryService _clientServiceCategoryService;

        public ClientServiceCategoryController(ILogger<ClientServiceCategoryController> logger
            , IClientServiceCategoryService clientServiceCategoryService)
        {
            _logger = logger;
            _clientServiceCategoryService = clientServiceCategoryService;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            try
            {
                var userResponse = await _clientServiceCategoryService.GetByIdAsync(id);
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
                var userResponse = await _clientServiceCategoryService.GetAllAsync(request);
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
        public async Task<IActionResult> AddAsync([FromForm] ClientServiceCategoryAddDTO request)
        {
            try
            {
                var userResponse = await _clientServiceCategoryService.AddAsync(UserId, request);
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
        public async Task<IActionResult> UpdateAsync([FromForm] ClientServiceCategoryUpdateDTO request)
        {
            try
            {
                var userResponse = await _clientServiceCategoryService.UpdateAsync(UserId, request);
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
                var response = await _clientServiceCategoryService.DeleteAsync(UserId, id);
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
