namespace QrToPay.Models.Responses;

public sealed class ActiveTicketsResponse
{
    public required int UserTicketId { get; init; }
    public required int GroupId { get; init; }
    public required IEnumerable<string> EntityNames { get; init; }
    public required string QrCode { get; init; }
    public required decimal Price { get; init; }
    public required int Points { get; init; }
    public required bool IsActive { get; init; }
}