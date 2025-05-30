using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Response
{
    public class PartnerResponseDTO
    {
        public int ID { get; set; } 

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string? Image { get; set; }

        public string? TwitterUrl { get; set; }

        public string? TiktokUrl { get; set; }

        public string? LinkedinUrl { get; set; }

        public string? MailUrl { get; set; }

        public string? WebsiteUrl { get; set; }

    }
}
