using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class UserTicket
{
    public int UserTicketId { get; set; }

    public int UserId { get; set; }

    public int? SkiResortId { get; set; }

    public int? FunFairId { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public int RemainingTokens { get; set; }

    public bool IsActive { get; set; }

    public string Qrcode { get; set; } = null!;

    public virtual FunFair? FunFair { get; set; }

    public virtual SkiSlope? SkiResort { get; set; }

    public virtual User User { get; set; } = null!;
}
