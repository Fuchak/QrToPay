namespace QrToPay.Api.Features.Auth.Login;

public sealed class LoginDto
{
    public required int UserId { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public decimal? AccountBalance { get; init; }
    public bool? IsActive { get; init; }
    public bool? IsBlocked { get; init; }
}