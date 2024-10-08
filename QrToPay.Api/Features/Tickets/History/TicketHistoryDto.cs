using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Features.Tickets.History;

public sealed class TicketHistoryDto
{
    public required string Date { get; init; }
    public required string Name { get; init; }
    public required decimal TotalPrice { get; init; }
    public required int Quantity { get; init; }
}