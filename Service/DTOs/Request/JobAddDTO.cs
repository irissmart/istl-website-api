using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Request
{
    public class JobAddDTO
    {
        public int JobCategoryId { get; set; }
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
        public List<JobTagAddRequestDTO> JobTags { get; set; } = new List<JobTagAddRequestDTO>();
    }

    public class JobTagAddRequestDTO
    {
        public int TagId { get; set; }
    }
}
