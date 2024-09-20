using MediatR;
using QrToPay.Api.Common.Filters;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.UserBalance.TopUp;

public sealed class TopUpRequestModel : IRequest<Result<decimal>>, IUserRequest
{
    public int UserId { get; set; }
    public required decimal Amount { get; init; }
}