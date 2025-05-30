using System.ComponentModel.DataAnnotations;

namespace Service.DTOs
{
    public class RefreshTokenDTO
    {
        [Required]
        public string Token { get; set; } = null!;
    }
}
