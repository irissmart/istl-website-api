using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class JobCategory
{
    public int Id { get; set; }

    public string JobCategoryName { get; set; } = null!;

    public string? ImageRelativePath { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
