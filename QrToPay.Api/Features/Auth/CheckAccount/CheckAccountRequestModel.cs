using MediatR;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Auth.CheckAccount;

public sealed class CheckAccountRequestModel : IRequest<Result<CheckAccountDto>>
{
    public string? Contact { get; init; }
    public required ChangeType ChangeType { get; init; }
}