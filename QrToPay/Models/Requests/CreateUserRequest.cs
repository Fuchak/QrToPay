namespace QrToPay.Models.Requests;

public sealed class CreateUserRequest
{
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public required string PasswordHash { get; init; }
}