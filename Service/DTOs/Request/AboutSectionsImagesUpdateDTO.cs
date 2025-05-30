using Microsoft.AspNetCore.Http;

namespace Service.DTOs
{
    public class AboutSectionsImagesUpdateDTO
    {
        public IFormFile? BannerImage { get; set; }
        public IFormFile? FirstSectionImage { get; set; }
        public IFormFile? SecondSectionImage { get; set; }
    }
}
