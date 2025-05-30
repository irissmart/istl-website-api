using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Analytic
{
    public int Id { get; set; }

    public int SectionId { get; set; }

    public string Number { get; set; } = null!;

    public string Title { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual Section Section { get; set; } = null!;
}
