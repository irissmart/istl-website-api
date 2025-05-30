using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Request
{
    public class ClientServiceCategoryAddDTO
    {
        public string ServiceCategoryName { get; set; } = null!;

        public string Description { get; set; } = null!;

        // Validate proper image.
        public IFormFile? Image { get; set; }
    }
}
