using MediatR;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Settings.EmailPhone;

public sealed class ChangeRequestModel : IRequest<Result<string>>
{
    public required int UserId { get; init; }
    public required string NewValue { get; init; }
    public required string Password { get; init; }
    public required ChangeType ChangeType { get; init; }
}