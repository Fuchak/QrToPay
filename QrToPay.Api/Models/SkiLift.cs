using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class SkiLift
{
    public int SkiLiftId { get; set; }

    public int SkiResortId { get; set; }

    public string LiftName { get; set; } = null!;

    public int TokensPerUse { get; set; }

    public string Qrcode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual SkiResort SkiResort { get; set; } = null!;
}
