using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Settings.Password;

public sealed class ChangePasswordRequestModel : IRequest<Result<SuccesMessageDto>>
{
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
    public required string ConfirmNewPassword { get; init; }
}