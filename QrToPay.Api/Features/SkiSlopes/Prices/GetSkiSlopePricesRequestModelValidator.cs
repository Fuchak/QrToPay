using FluentValidation;

namespace QrToPay.Api.Features.SkiSlopes.Prices;

public class GetSkiSlopePricesRequestModelValidator : AbstractValidator<GetSkiSlopePricesRequestModel>
{
    public GetSkiSlopePricesRequestModelValidator()
    {
        RuleFor(x => x.SkiResortId).GreaterThan(0).WithMessage("SkiResortId jest wymagany.");
    }
}