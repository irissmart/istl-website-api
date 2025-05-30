using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class JobStatus
{
    public int Id { get; set; }

    public string JobStatusName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
