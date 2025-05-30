using Framework.Model;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost]
        [Route("api/[controller]/token")]
        public async Task<IActionResult> AuthenticateAsync(AuthRequest request)
        {
            try
            {
                var response = await _authService.AuthenticateAsync(request);
                return StatusCode(response.Item2, response.Item1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("api/[controller]/refresh-token")]
        public async Task<IActionResult> GenreateRefreshTokenAsync([FromBody] RefreshTokenDTO request)
        {
            try
            {
                var response = await _authService.RefreshTokenAsync(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
