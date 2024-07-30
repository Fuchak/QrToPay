namespace QrToPay.Models.Requests;

public sealed class HelpFormRequest
{
    public required string UserName { get; init; }
    public required string UserEmail { get; init; }
    public required string Subject { get; init; }
    public required string Description { get; init; }
    public required string Status { get; init; }
}