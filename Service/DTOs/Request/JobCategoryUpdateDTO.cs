using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Request
{
    public class JobCategoryUpdateDTO
    {
        public int Id { get; set; }
        public string JobCategoryName { get; set; } = null!;

        // Validate proper image.
        public IFormFile? Image { get; set; }
    }
}
