using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Testimonial
{
    public int Id { get; set; }

    public string Comment { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public string ClientOccupation { get; set; } = null!;

    public string? ImageRelativePath { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
