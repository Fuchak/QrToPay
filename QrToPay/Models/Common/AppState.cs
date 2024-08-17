using QrToPay.Models.Enums;

namespace QrToPay.Models.Common;
public sealed class AppState
{
    public string? CityName { get; set; }
    public Guid ServiceId { get; set; }
    public int? AttractionId { get; set; }
    public string? ResortName { get; set; }
    public decimal Price { get; set; }
    public int Points { get; set; }
}