namespace QrToPay.Api.Requests;

public sealed class CreateUserRequestModel
{
    public string? PhoneNumber { get; init; }
    public string? Email { get; init; }
    public required string PasswordHash { get; init; }
}