using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Entities;

namespace Service.DTOs.Response
{
    public class JobDTO
    {
        public int Id { get; set; }
        public int JobCategoryId { get; set; }
        public string JobCategoryName { get; set; }
        public string? JobCategoryImage { get; set; }
        public string Title { get; set; }
        public string? Currency { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public int JobStatusId { get; set; }
        public string JobStatusName { get; set; }
        public List<JobTagNameDTO> JobTags { get; set; }
        public int ExperienceYearsRequired { get; set; }
        public int Vacancies { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Description { get; set; }
        public string Responsibilities { get; set; }
        public string City { get; set; }

        public string Country { get; set; }

        public int TotalJobApplications { get; set; }
    }
}
