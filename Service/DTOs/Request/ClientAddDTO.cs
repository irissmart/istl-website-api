using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Service.DTOs.Request
{
    public class ClientAddDTO
    {
        public string Name { get; set; } = null!;

        public IFormFile Image { get; set; } = null!;

        public string Url { get; set; } = null!;
    }
}
