using FluentValidation;

namespace QrToPay.Api.Features.SkiResorts.Prices;

public class GetSkiResortPricesRequestModelValidator : AbstractValidator<GetSkiResortPricesRequestModel>
{
    public GetSkiResortPricesRequestModelValidator()
    {
        RuleFor(x => x.SkiResortId).GreaterThan(0).WithMessage("SkiResortId jest wymagany.");
    }
}