using System;
using System.Collections.Generic;

namespace Infrastructure.Entities;

public partial class ContactInformation
{
    public int Id { get; set; }

    public string PhoneNo { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Address { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<SocialLink> SocialLinks { get; set; } = new List<SocialLink>();
}
