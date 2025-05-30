using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Response
{
    public class StatsDTO
    {
        public int TotalJobs { get; set; }

        public int AppliedJobs { get; set; }

        public int TotalVisitors { get; set; }
    }
}
