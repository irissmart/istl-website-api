using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Request
{
    public class ClientServiceAddDTO
    {
        public int ClientServiceCategoryId { get; set; }

        public string ServiceName { get; set; } = null!;

        public string Description { get; set; } = null!;

        // Validate proper image.
        public IFormFile Image { get; set; } = null!;
    }
}
