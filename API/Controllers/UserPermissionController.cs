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
    public class UserPermission : BaseController
    {
        private readonly ILogger<UserPermission> _logger;
        private readonly IUserPermission _userPermissionService;

        public UserPermission(ILogger<UserPermission> logger, IUserPermission userPermissionService)
        {
            _logger = logger;
            _userPermissionService = userPermissionService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/[Controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] BaseRequest request)
        {
            try
            {
                var response = await _userPermissionService.GetAllAsync(request);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("api/[Controller]")]
        public async Task<IActionResult> GetAllByUserIdAsync([FromQuery] BaseRequest request)
        {
            try
            {
                var response = await _userPermissionService.GetAllByUserIdAsync(UserId, request);
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
