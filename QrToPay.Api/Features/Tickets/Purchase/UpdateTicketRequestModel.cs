namespace QrToPay.Api.Features.Tickets.Purchase;

public class UpdateTicketRequestModel
{
    public required int UserId { get; init; }
    public required Guid EntityId { get; init; }
    public required int Quantity { get; init; }
    public required int Tokens { get; init; }
    public required decimal TotalPrice { get; init; }
}