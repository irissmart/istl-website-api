using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.Request
{
    public class ChangePasswordDTO
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
