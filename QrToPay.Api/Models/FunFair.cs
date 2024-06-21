using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class FunFair
{
    public int FunFairId { get; set; }

    public int CityId { get; set; }

    public string ParkName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();

    public virtual City City { get; set; } = null!;

    public virtual ICollection<FunFairPrice> FunFairPrices { get; set; } = new List<FunFairPrice>();

    public virtual ICollection<TicketHistory> TicketHistories { get; set; } = new List<TicketHistory>();

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
