using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class UserRole
{
    public int Id { get; set; }

    public string UserRoleName { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
