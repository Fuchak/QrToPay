namespace QrToPay.Api.Requests.Settings;
//TODO : do rozdzielenia na enuma i model
public sealed class VerifyRequestModel
{
    public required int UserId { get; init; }
    public required string VerificationCode { get; init; }
    public required ChangeType ChangeType { get; init; }
}
public enum ChangeType
{
    Email,
    Phone
}