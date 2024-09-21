using FluentValidation;

namespace QrToPay.Api.Features.UserBalance.TopUp;
public class TopUpRequestModelValidator : AbstractValidator<TopUpRequestModel>
{
    public TopUpRequestModelValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .PrecisionScale(12, 2, true)
            .WithMessage("Kwota doładowania musi być większa niż 0.");
    }
}