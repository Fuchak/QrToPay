using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.UserBalance.CheckBalance;

public sealed class GetUserBalanceRequestModel : IRequest<Result<UserBalanceDto>>
{
    [FromRoute(Name = "userId")]
    public required int UserId { get; init; }
}