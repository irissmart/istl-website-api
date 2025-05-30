using Framework.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs.Request;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class NotificationController : BaseController
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationService _notificationService;

        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/[controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] BaseRequest dto)
        {
            try
            {
                var userResponse = await _notificationService.GetAllAsync(UserId, dto);
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
        public async Task<IActionResult> UpdateAsync()
        {
            try
            {
                await _notificationService.UpdateAsync(UserId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
