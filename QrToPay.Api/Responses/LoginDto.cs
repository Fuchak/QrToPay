namespace QrToPay.Api.Responses;

public sealed class LoginDto
{
    public required int UserId { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public decimal? AccountBalance { get; init; }
}