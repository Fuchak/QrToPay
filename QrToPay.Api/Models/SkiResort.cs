using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class SkiResort
{
    public int SkiResortId { get; set; }

    public int ServiceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ServiceCategory Service { get; set; } = null!;

    public virtual ICollection<SkiLift> SkiLifts { get; set; } = new List<SkiLift>();

    public virtual ICollection<SkiResortPrice> SkiResortPrices { get; set; } = new List<SkiResortPrice>();
}
