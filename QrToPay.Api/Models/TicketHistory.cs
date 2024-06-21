using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class TicketHistory
{
    public int HistoryId { get; set; }

    public int UserId { get; set; }

    public int? SkiResortId { get; set; }

    public int? FunFairId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual FunFair? FunFair { get; set; }

    public virtual SkiSlope? SkiResort { get; set; }

    public virtual User User { get; set; } = null!;
}
