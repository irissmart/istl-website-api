using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Response
{
    public class VisitStatsDto
    {
        public int TotalVisits { get; set; }
        public int UniqueVisitors { get; set; }
        public int NewVisitors { get; set; }
        public int PagesVisited { get; set; }
        public double AverageVisitsPerVisitor { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
