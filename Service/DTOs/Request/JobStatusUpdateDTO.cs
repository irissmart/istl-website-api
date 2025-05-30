using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Request
{
    public class JobStatusUpdateDTO
    {
        public int JobId { get; set; }
        public int JobStatusId { get; set; }
    }
}
