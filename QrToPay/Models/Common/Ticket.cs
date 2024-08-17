namespace QrToPay.Models.Common;

public sealed class Ticket
{
    //public Guid ServiceId { get; init; }
    public string? CityName { get; init; }
    public int UserId { get; init; }
    public string? EntityName { get; init; }
    public string? QrCode { get; init; }
    public decimal Price { get; init; }
    public int Points { get; init; }
}