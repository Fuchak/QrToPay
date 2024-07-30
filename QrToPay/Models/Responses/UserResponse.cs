namespace QrToPay.Models.Responses;

public sealed class UserResponse
{
    public required int UserId { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public decimal? AccountBalance { get; init; }
}