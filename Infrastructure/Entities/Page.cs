using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Page
{
    public int Id { get; set; }

    public string PageName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}
