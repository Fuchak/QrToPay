using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.UserBalance.TopUp;

public sealed class TopUpRequestModel : IRequest<Result<decimal>>
{
    public required decimal Amount { get; init; }
}