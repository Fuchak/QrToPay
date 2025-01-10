namespace QrToPay.Models.Requests;

public sealed class UpdateTicketRequest
{
    public required int ServiceId { get; init; }
    public required int Quantity { get; init; }
    public required int Tokens { get; init; }
    public required string TotalPrice { get; init; }
}