using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Request
{
    public class ClientServiceCategoryUpdateDTO
    {
        public int Id { get; set; }
        public string ServiceCategoryName { get; set; } = null!;

        public string Description { get; set; } = null!;

        // Validate proper image.
        public IFormFile? Image { get; set; }
    }
}
