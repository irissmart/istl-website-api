using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Request
{
    public class JobUpdateDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Currency { get; set; }
        public decimal MinSalary { get; set; }
        public decimal MaxSalary { get; set; }
        public int JobStatusId { get; set; }
        public int ExperienceYearsRequired { get; set; }
        public int Vacancies { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Description { get; set; }
        public string Responsibilities { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<JobTagUpdateRequestDTO> JobTags { get; set; } = new List<JobTagUpdateRequestDTO>();
    }

    public class JobTagUpdateRequestDTO
    {
        public int TagId { get; set; }
    }
}
