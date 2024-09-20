using MediatR;
using QrToPay.Api.Common.Filters;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.UserBalance.CheckBalance;

public sealed class GetUserBalanceRequestModel : IRequest<Result<UserBalanceDto>>, IUserRequest
{
    public int UserId { get; set; }
}