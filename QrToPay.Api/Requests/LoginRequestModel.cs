namespace QrToPay.Api.Requests;

public sealed class LoginRequestModel
{
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? PasswordHash { get; init; }
}