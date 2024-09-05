using System.Text.Json.Serialization;

namespace QrToPay.Models.Responses;

public sealed class CreateUserResponse
{
    public required string EmailOrPhone { get; init; }
    public required string VerificationCode { get; init; }
}