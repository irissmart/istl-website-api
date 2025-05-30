using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Request
{
    public class ManagementSectionImageUpdateDTO
    {
        public IFormFile? BannerImage { get; set; }
        public IFormFile? FirstSectionImage { get; set; }
        public IFormFile? SecondSectionImage { get; set; }
        public IFormFile? ThirdSectionImage { get; set; }
    }
}
