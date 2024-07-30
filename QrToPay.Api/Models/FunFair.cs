namespace QrToPay.Api.Models;

public partial class FunFair
{
    public int FunFairId { get; set; }

    public Guid EntityId { get; set; }

    public string CityName { get; set; } = null!;

    public string ParkName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Entity Entity { get; set; } = null!;

    public virtual ICollection<FunFairAttraction> FunFairAttractions { get; set; } = new List<FunFairAttraction>();

    public virtual ICollection<FunFairPrice> FunFairPrices { get; set; } = new List<FunFairPrice>();
}
