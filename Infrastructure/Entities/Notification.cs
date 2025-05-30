using System;
using System.Collections.Generic;

namespace Infrastructure.ModelsX;

public partial class Notification
{
    public int Id { get; set; }

    public int ReceiverId { get; set; }

    public string Message { get; set; } = null!;

    public int TypeId { get; set; }

    public bool IsRead { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
