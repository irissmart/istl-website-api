using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Request
{
    public class JobCategoryAddDTO
    {
        public string JobCategoryName { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}
