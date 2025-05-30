using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Job
{
    public int Id { get; set; }

    public int JobCategoryId { get; set; }

    public string Title { get; set; } = null!;

    public decimal? MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public int JobStatusId { get; set; }

    public int ExperienceYearsRequired { get; set; }

    public int Vacancies { get; set; }

    public DateTime ExpiresOn { get; set; }

    public string Description { get; set; } = null!;

    public string Responsibilities { get; set; } = null!;

    public bool? IsEnabled { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? Currency { get; set; }

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    public virtual JobCategory JobCategory { get; set; } = null!;

    public virtual JobStatus JobStatus { get; set; } = null!;

    public virtual ICollection<JobTag> JobTags { get; set; } = new List<JobTag>();
}
