using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using Framework.Model;
using Framework.Service;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Request;
using Service.Interface;

namespace API.Controllers
{
    [ApiController]
    public class ContactRequestController : BaseController
    {
        private IContactRequestService _contactRequestService { get; set; }
        private ILogger<ContactRequestController> _logger { get; set; }
        public ContactRequestController(IrisContext context, IContactRequestService contactRequestService, ILogger<ContactRequestController> logger)
        {
            _contactRequestService = contactRequestService;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/[controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] BaseRequest? request)
        {
            try
            {
                var userResponse = await _contactRequestService.GetAllAsync(request);
                return GenerateResponse(userResponse);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> GetByIdAsync([FromQuery]int id)
        {
            try
            {
                var userResponse = await _contactRequestService.GetByIdAsync(id);
                return GenerateResponse(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/[controller]")]
        public async Task<IActionResult> AddAsync([FromBody] ContactRequestAddDTO contactRequestAddDTO)
        {
            try
            {
                var userResponse = await _contactRequestService.AddAsync(contactRequestAddDTO);
                return GenerateResponse(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
