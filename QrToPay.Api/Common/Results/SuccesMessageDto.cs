namespace QrToPay.Api.Common.Results;

public sealed class SuccesMessageDto
{
    public string? Message { get; init; }
    public bool? IsSuccessful { get; init; }
}