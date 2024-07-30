namespace QrToPay.Models.Requests;

//Jest git używa tylko init ale debug używa geta dlatego się podświetla w riderze...
public sealed class UpdateTicketRequest
{
    public required int UserId { get; init; }
    public required Guid EntityId { get; init; }
    public required int Quantity { get; init; }
    public required int Tokens { get; init; }
    public required string TotalPrice { get; init; }
}