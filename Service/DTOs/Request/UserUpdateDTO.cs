using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Request
{
    public class UserUpdateDTO
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        // Validate proper image.
        public IFormFile? Image { get; set; }
    }
}
