namespace QrToPay.Api.Requests;

public sealed class UserExistRequestModel
{
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
}