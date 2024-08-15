using FluentValidation;

namespace QrToPay.Api.Features.UserBalance.CheckBalance;
public class GetUserBalanceRequestModelValidator : AbstractValidator<GetUserBalanceRequestModel>
{
    public GetUserBalanceRequestModelValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("Identyfikator użytkownika musi być większy niż 0.");
    }
}