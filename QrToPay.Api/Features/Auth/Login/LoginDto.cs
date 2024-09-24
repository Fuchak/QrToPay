namespace QrToPay.Api.Features.Auth.Login;

public sealed class LoginDto
{
    public required string Token { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
}