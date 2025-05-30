using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class UserPermission
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string ModuleName { get; set; } = null!;

    public bool CanView { get; set; }

    public bool CanCreate { get; set; }

    public bool CanUpdate { get; set; }

    public bool CanDelete { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual User User { get; set; } = null!;
}
