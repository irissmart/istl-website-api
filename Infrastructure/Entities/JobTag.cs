using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class JobTag
{
    public int Id { get; set; }

    public int JobId { get; set; }

    public int TagId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Job Job { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
