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
    public class TeamMemberController : BaseController
    {
        private readonly ILogger<TeamMemberController> _logger;
        private readonly ITeamMemberService _teamMemberService;

        public TeamMemberController(ILogger<TeamMemberController> logger, ITeamMemberService teamMemberService)
        {
            _logger = logger;
            _teamMemberService = teamMemberService;
        }

        [HttpGet]
        [Route("api/[Controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] BaseRequest request)
        {
            try
            {
                var response = await _teamMemberService.GetAllAsync(request);
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
                var response = await _teamMemberService.GetByIdAsync(id);
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
        public async Task<IActionResult> AddAsync([FromForm] TeamMemberAddDTO request)
        {
            try
            {
                var response = await _teamMemberService.AddAsync(UserId, request);
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
        public async Task<IActionResult> UpdateAsync(TeamMemberUpdateDTO request)
        {
            try
            {
                var response = await _teamMemberService.UpdateAsync(UserId, request);
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
        [Route("api/[controller]")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var response = await _teamMemberService.DeleteAsync(UserId, id);
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
