using FluentValidation;
using QrToPay.Api.Features.SkiResorts.Prices;

namespace QrToPay.Api.Features.FunFairs.Prices;

public class GetFunFairPricesRequestModelValidator : AbstractValidator<GetFunFairPricesRequestModel>
{
    public GetFunFairPricesRequestModelValidator()
    {
        RuleFor(x => x.FunFairId).GreaterThan(0).WithMessage("FunFairId jest wymagany.");
    }
}