using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class SocialLink
{
    public int Id { get; set; }

    public int ContactInformationId { get; set; }

    public string PlatformName { get; set; } = null!;

    public string Url { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ContactInformation ContactInformation { get; set; } = null!;
}
