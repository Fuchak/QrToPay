namespace QrToPay.Api.Features.Tickets.Active;

public sealed class TicketDto
{
    public required int UserTicketId { get; init; }
    public required int UserId { get; init; }
    public required Guid EntityId { get; init; }
    public required string EntityType { get; init; }
    public required string EntityName { get; init; }
    public required string CityName { get; init; }
    public required string QrCode { get; init; }
    public required decimal Price { get; init; }
    public required int Points { get; init; }
    public required bool IsActive { get; init; }
}