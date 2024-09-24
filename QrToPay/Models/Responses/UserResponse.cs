namespace QrToPay.Models.Responses;

public sealed class UserResponse
{
    public required string Token { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
}