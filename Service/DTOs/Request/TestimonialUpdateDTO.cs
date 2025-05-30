using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Request
{
    public class TestimonialUpdateDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; } = null!;

        public string ClientName { get; set; } = null!;

        public string ClientOccupation { get; set; } = null!;

        // Validate proper image.
        public IFormFile? Image { get; set; }
    }
}
