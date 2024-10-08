using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class CompanyGroupMember
{
    public int GroupMemberId { get; set; }

    public int GroupId { get; set; }

    public int ServiceId { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual CompanyGroup Group { get; set; } = null!;

    public virtual ServiceCategory Service { get; set; } = null!;
}
