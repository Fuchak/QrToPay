﻿using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class SkiResortPrice
{
    public int SkiResortPriceId { get; set; }

    public int SkiResortId { get; set; }

    public int Tokens { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual SkiResort SkiResort { get; set; } = null!;
}
