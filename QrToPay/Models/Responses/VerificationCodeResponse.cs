using System.Text.Json.Serialization;

namespace QrToPay.Models.Responses;

public sealed class VerificationCodeResponse
{
    public required string VerificationCode { get; init; }
}