namespace QrToPay.Api.Features.UserBalance.CheckBalance;

public sealed class UserBalanceDto
{
    public required decimal AccountBalance { get; init; }
}