using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class User
{
    public int Id { get; set; }

    public int UserRoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string SaltHash { get; set; } = null!;

    public bool IsVerified { get; set; }

    public string Token { get; set; } = null!;

    public DateTime TokenExpiry { get; set; }

    public string? ProfileImageRelativePath { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    
    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    public virtual UserRole UserRole { get; set; } = null!;
}
