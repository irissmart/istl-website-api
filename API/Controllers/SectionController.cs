using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.DTOs.Request;
using Service.DTOs.Response;
using Service.Interface;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    public class SectionController : BaseController
    {
        private readonly ILogger<SectionController> _logger;
        private readonly ISectionService _sectionService;

        public SectionController(ILogger<SectionController> logger
            , ISectionService sectionService)
        {
            _logger = logger;
            _sectionService = sectionService;
        }

        [HttpGet]
        [Route("api/[controller]s")]
        public async Task<IActionResult> GetAllAsync([FromQuery] int pageId)
        {
            try
            {
                var userResponse = await _sectionService.GetByPageIdAsync(pageId);
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
        [Route("api/[controller]s")]
        public async Task<IActionResult> UpdateAsync([FromBody] SectionsUpdateDTO request)
        {
            try
            {
                var userResponse = await _sectionService.UpdateAsync(UserId, request);
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
        [Route("api/[controller]s/home")]
        public async Task<IActionResult> UpdateHomeSectionsAsync([FromBody] HomeSectionsUpdateDTO request)
        {
            try
            {
                var userResponse = await _sectionService.UpdateHomeSectionsAsync(UserId, request);
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
        [Route("api/[controller]s/home/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateHomeSectionsImagesAsync([FromForm] HomeSectionsImagesUpdateDTO request)
        {
            try
            {
                var userResponse = await _sectionService.UpdateHomeSectionsImagesAsync(UserId, request);
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
        [Route("api/[controller]s/service/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateServiceSectionsImagesAsync([FromForm] ServiceSectionsImagesUpdateDTO request)
        {
            try
            {
                var userResponse = await _sectionService.UpdateServiceSectionsImagesAsync(UserId, request);
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
        [Route("api/[controller]s/about/images")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateAboutSectionsImagesAsync([FromForm] AboutSectionsImagesUpdateDTO request)
        {
            try
            {
                var userResponse = await _sectionService.UpdateAboutSectionsImagesAsync(UserId, request);
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
        [Route("api/[controller]s/contact/image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateContactSectionImageAsync([FromForm] ContactSectionImageUpdateDTO request)
        {
            try
            {
                var userResponse = await _sectionService.UpdateContactSectionImageAsync(UserId, request);
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
        [Route("api/[controller]s/management/image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateManagementSectionImageAsync([FromForm] ManagementSectionImageUpdateDTO request)
        {
            try
            {
                var userResponse = await _sectionService.UpdateManagementSectionImageAsync(UserId, request);
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
        [Route("api/[controller]s/vacancy/image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateVacancySectionImageAsync([FromForm] VacancySectionImageUpdateDTO request)
        {
            try
            {
                var userResponse = await _sectionService.UpdateVacancySectionImageAsync(UserId, request);
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
        [Route("api/[controller]s/partner/image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePartnerSectionImageAsync([FromForm] PartnerSectionImageUpdateDTO request)
        {
            try
            {
                var userResponse = await _sectionService.UpdatePartnerSectionImageAsync(UserId, request);
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
        [Route("api/[controller]s/sitemap/image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateSitemapSectionImageAsync([FromForm] SitemapSectionImageUpdateDTO request)
        {
            try
            {
                var userResponse = await _sectionService.UpdateSitemapSectionImageAsync(UserId, request);
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
