namespace QrToPay.Models.Responses;

public sealed class HistoryItemResponse
{
    public required string Date { get; init; }
    public required string Type { get; init; }
    public required string Name { get; init; }
    public required decimal TotalPrice { get; init; }
}