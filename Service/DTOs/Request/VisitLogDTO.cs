using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Request
{
    public class VisitLogDto
    {
        public string PageUrl { get; set; }
        public string UserAgent { get; set; }
        public string Referrer { get; set; }
        public string ClientId { get; set; }
        public string SessionId { get; set; }
    }
}
