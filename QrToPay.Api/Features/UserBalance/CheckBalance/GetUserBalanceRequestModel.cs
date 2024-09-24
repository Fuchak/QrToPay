using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.UserBalance.CheckBalance;

public sealed class GetUserBalanceRequestModel : IRequest<Result<UserBalanceDto>>
{

}