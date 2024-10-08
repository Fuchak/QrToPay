using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class CompanyGroup
{
    public int GroupId { get; set; }

    public string GroupName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<CompanyGroupMember> CompanyGroupMembers { get; set; } = new List<CompanyGroupMember>();

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
