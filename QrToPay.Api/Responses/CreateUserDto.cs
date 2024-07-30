namespace QrToPay.Api.Responses;

public sealed class CreateUserDto
{
    public required string VerificationCode { get; init; }
}