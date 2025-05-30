using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Request
{
    public class ServiceAddDTO
    {
        public int SectionId { get; set; }

        public string Title { get; set; } = null!;

        public string? LogoRelativePath { get; set; }
    }
}
