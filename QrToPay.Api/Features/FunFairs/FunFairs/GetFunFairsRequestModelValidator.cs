using FluentValidation;

namespace QrToPay.Api.Features.FunFairs.FunFairs;

public class GetFunFairsRequestModelValidator: AbstractValidator<GetFunFairsRequestModel>
{
    public GetFunFairsRequestModelValidator()
    {
        RuleFor(x => x.CityName).NotEmpty().WithMessage("CityName jest wymagane.");
    }
}