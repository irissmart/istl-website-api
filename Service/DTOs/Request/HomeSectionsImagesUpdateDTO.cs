using Microsoft.AspNetCore.Http;

namespace Service.DTOs
{
    public class HomeSectionsImagesUpdateDTO
    {
        public IFormFile? BackgroundImage { get; set; }
        public List<IFormFile>? ServiceImages { get; set; }
        public List<IFormFile>? DetailedServiceImages { get; set; }
    }
}
