using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class SkiSlopePrice
{
    public int SkiSlopePriceId { get; set; }

    public int SkiResortId { get; set; }

    public int Tokens { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual SkiSlope SkiResort { get; set; } = null!;
}
