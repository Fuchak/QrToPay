namespace QrToPay.Models.Requests;

public sealed class HistoryRequest
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
}