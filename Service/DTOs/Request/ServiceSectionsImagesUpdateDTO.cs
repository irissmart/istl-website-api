using Microsoft.AspNetCore.Http;

namespace Service.DTOs
{
    public class ServiceSectionsImagesUpdateDTO
    {
        public IFormFile? BannerImage { get; set; }
        public IFormFile? FirstSectionImage { get; set; }
    }
}
