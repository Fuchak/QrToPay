namespace QrToPay.Api.Models;

public partial class FunFairAttraction
{
    public int AttractionId { get; set; }

    public int FunFairId { get; set; }

    public string AttractionName { get; set; } = null!;

    public int TokensPerUse { get; set; }

    public string Qrcode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual FunFair FunFair { get; set; } = null!;
}
