using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class SkiSlope
{
    public int SkiResortId { get; set; }

    public int CityId { get; set; }

    public string ResortName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<SkiLift> SkiLifts { get; set; } = new List<SkiLift>();

    public virtual ICollection<SkiSlopePrice> SkiSlopePrices { get; set; } = new List<SkiSlopePrice>();

    public virtual ICollection<TicketHistory> TicketHistories { get; set; } = new List<TicketHistory>();

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
