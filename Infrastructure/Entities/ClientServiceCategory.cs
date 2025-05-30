using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class ClientServiceCategory
{
    public int Id { get; set; }

    public string ServiceCategoryName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? CategoryImagePath { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<ClientService> ClientServices { get; set; } = new List<ClientService>();
}
