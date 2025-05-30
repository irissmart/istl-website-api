using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Response
{
    public class ClientResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Url { get; set; }
    }
}
