using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class FunFair
{
    public int FunFairId { get; set; }

    public int ServiceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<FunFairAttraction> FunFairAttractions { get; set; } = new List<FunFairAttraction>();

    public virtual ICollection<FunFairPrice> FunFairPrices { get; set; } = new List<FunFairPrice>();

    public virtual ServiceCategory Service { get; set; } = null!;
}
