using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class Partner
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? Image { get; set; }

    public string? TwitterUrl { get; set; }

    public string? TiktokUrl { get; set; }

    public string? LinkedinUrl { get; set; }

    public string? MailUrl { get; set; }

    public string? WebsiteUrl { get; set; }
}
