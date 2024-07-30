namespace QrToPay.Api.Models;

public partial class SkiSlope
{
    public int SkiResortId { get; set; }

    public Guid EntityId { get; set; }

    public string CityName { get; set; } = null!;

    public string ResortName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    public virtual ICollection<SkiLift> SkiLifts { get; set; } = new List<SkiLift>();

    public virtual ICollection<SkiSlopePrice> SkiSlopePrices { get; set; } = new List<SkiSlopePrice>();
}
