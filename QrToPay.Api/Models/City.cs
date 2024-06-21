using System;
using System.Collections.Generic;

namespace QrToPay.Api.Models;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<FunFair> FunFairs { get; set; } = new List<FunFair>();

    public virtual ICollection<SkiSlope> SkiSlopes { get; set; } = new List<SkiSlope>();
}
