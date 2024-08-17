﻿using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class TicketHistory
{
    public int HistoryId { get; set; }

    public int UserId { get; set; }

    public Guid ServiceId { get; set; }

    public DateTime PurchaseDate { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public virtual ServiceCategory Service { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
