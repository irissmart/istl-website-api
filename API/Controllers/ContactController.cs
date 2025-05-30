using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.DTOs.Response;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class ContactController : BaseController
    {
        private readonly ILogger<ContactController> _logger;
        private readonly IContactService _contactService;

        public ContactController(ILogger<ContactController> logger
            , IContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var userResponse = await _contactService.GetAsync();
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
        public async Task<IActionResult> UpdateAsync([FromBody] ContactDTO request)
        {
            try
            {
                var userResponse = await _contactService.UpdateAsync(UserId, request);
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
