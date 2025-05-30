using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Service.DTOs.Request
{
    public class JobApplicationAddDTO
    {
        [Required]
        public int JobId { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string Contact { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Message { get; set; } = null!;

        // File validation.

        [Required]
        public IFormFile Document { get; set; } = null!;
    }
}
