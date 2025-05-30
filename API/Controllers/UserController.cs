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
    public class UserController : BaseController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(ILogger<UserController> logger
            , IUserService userService
            , IConfiguration configuration)
        {
            _logger = logger;
            _userService = userService;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        [Route("api/[Controller]")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            try
            {
                var response = await _userService.GetByIdAsync(id);
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
        public async Task<IActionResult> UpdateAsync(UserUpdateDTO request)
        {
            try
            {
                var response = await _userService.UpdateAsync(UserId, request);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("api/[controller]/forget-password")]
        public async Task<IActionResult> ForgetPasswordAsync(string email)
        {
            try
            {
                var response = await _userService.ForgetPasswordAsync(email);
                return GenerateResponse(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/[controller]/verify-token")]
        public async Task<IActionResult> VerifyTokenAsync(string token)
        {
            try
            {
                var response = await _userService.VerifyTokenAsync(token);

                if (response.Success)
                {
                    return Redirect($"{_configuration["PasswordResetUrl"]}?token={token}");
                }
                else
                {
                    return Redirect($"{_configuration["PasswordResetLinkExpired"]}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("api/[controller]/reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] string password, string token)
        {
            try
            {
                var response = await _userService.ResetPasswordAsync(password, token);
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
        [Route("api/[controller]/change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO request)
        {
            try
            {
                var response = await _userService.ChangePasswordAsync(UserId, request);
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
