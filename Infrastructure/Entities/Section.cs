using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Section
{
    public int Id { get; set; }

    public int PageId { get; set; }

    public string SectionName { get; set; } = null!;

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ButtonText { get; set; }

    public string? BackgroundImageRelativePath { get; set; }

    public string? SectionImageRelativePath { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Analytic> Analytics { get; set; } = new List<Analytic>();

    public virtual ICollection<DetailedService> DetailedServices { get; set; } = new List<DetailedService>();

    public virtual Page Page { get; set; } = null!;

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual ICollection<Step> Steps { get; set; } = new List<Step>();
}
