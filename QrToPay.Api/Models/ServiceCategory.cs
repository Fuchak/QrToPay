﻿using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class ServiceCategory
{
    public int ServiceId { get; set; }

    public int ServiceType { get; set; }

    public string CityName { get; set; } = null!;

    public string ServiceName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<CompanyGroupMember> CompanyGroupMembers { get; set; } = new List<CompanyGroupMember>();

    public virtual ICollection<FunFair> FunFairs { get; set; } = new List<FunFair>();

    public virtual ICollection<SkiResort> SkiResorts { get; set; } = new List<SkiResort>();

    public virtual ICollection<TicketHistory> TicketHistories { get; set; } = new List<TicketHistory>();
}
