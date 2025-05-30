using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Expires { get; set; }

    public bool IsRevoked { get; set; }

    public bool IsUsed { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Revoked { get; set; }

    public DateTime? Used { get; set; }

    public virtual User User { get; set; } = null!;
}
