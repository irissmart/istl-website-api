using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Request
{
    public class TagUpdateDTO
    {
        public int Id { get; set; }

        public string? TagName { get; set; } = null!;

        public bool? IsEnabled { get; set; } = null!;
    }
}
