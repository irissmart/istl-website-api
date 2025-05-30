using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class ClientService
{
    public int Id { get; set; }

    public int ClientServiceCategoryId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ServiceImagePath { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ClientServiceCategory ClientServiceCategory { get; set; } = null!;
}
