using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Auth.ResetPassword;

public sealed class ResetPasswordRequestModel : IRequest<Result<string>>
{
    public required string EmailOrPhone { get; init; }
    public required string VerificationCode { get; init; }
    public required string NewPassword { get; init; }
}