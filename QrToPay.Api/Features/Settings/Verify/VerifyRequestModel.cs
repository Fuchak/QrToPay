using MediatR;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Settings.Verify;
public sealed class VerifyRequestModel : IRequest<Result<string>>
{
    public required int UserId { get; init; }
    public required string VerificationCode { get; init; }
    public required ChangeType ChangeType { get; init; }
}