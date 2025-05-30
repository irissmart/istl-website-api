using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Request
{
    public class PartnerAddDTO
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public IFormFile? Image { get; set; }

        public string? TwitterUrl { get; set; }

        public string? TiktokUrl { get; set; }

        public string? LinkedinUrl { get; set; }

        public string? MailUrl { get; set; }

        public string? WebsiteUrl { get; set; }
    }
}
