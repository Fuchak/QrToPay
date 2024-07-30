namespace QrToPay.Api.Requests.Settings;

public sealed class ChangeRequestModel
{
    public required int UserId { get; init; }
    public required string NewValue { get; init; }
    public required string Password { get; init; }
    public required ChangeType ChangeType { get; init; }
}