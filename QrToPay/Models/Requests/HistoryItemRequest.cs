namespace QrToPay.Models.Requests;

public sealed class HistoryItemRequest
{
    public required string Date { get; init; }
    public required string Type { get; init; }
    public required string Name { get; init; }
    public required decimal TotalPrice { get; init; }
}