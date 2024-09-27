using FluentValidation;

namespace QrToPay.Api.Features.UserBalance.CheckBalance;
public class GetUserBalanceRequestModelValidator : AbstractValidator<GetUserBalanceRequestModel>
{
    public GetUserBalanceRequestModelValidator()
    {
    }
}