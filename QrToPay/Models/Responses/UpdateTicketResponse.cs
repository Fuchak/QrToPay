namespace QrToPay.Models.Responses;

public sealed class UpdateTicketResponse
{
    public required string QrCode { get; init; }
}