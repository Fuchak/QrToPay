﻿namespace QrToPay.Api.Models;

public partial class Entity
{
    public required Guid EntityId { get; set; }

    public required string EntityType { get; set; }

    public required string EntityName { get; set; }

    public required string CityName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<FunFair> FunFairs { get; set; } = new List<FunFair>();

    public virtual ICollection<SkiSlope> SkiSlopes { get; set; } = new List<SkiSlope>();

    public virtual ICollection<TicketHistory> TicketHistories { get; set; } = new List<TicketHistory>();

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
