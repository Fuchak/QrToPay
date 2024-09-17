using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Register.VerifyCreateUser;

public sealed class VerifyCreateUserRequestModel : IRequest<Result<SuccesMessageDto>>
{
    public required string EmailOrPhone { get; init; }
    public required string VerificationCode { get; init; }
}