using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class FunFairPrice
{
    public int FunFairPriceId { get; set; }

    public int FunFairId { get; set; }

    public int Tokens { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual FunFair FunFair { get; set; } = null!;
}
