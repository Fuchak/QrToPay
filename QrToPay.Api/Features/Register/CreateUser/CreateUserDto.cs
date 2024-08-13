namespace QrToPay.Api.Features.Register.CreateUser;

public sealed class CreateUserDto
{
    public required string EmailOrPhone { get; init; }
    public required string VerificationCode { get; init; }
}