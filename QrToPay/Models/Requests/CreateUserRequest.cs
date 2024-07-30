namespace QrToPay.Models.Requests;

public sealed class CreateUserRequest
{
    public required string EmailOrPhone { get; init; }
    public required string PasswordHash { get; init; }
}