using FluentValidation;

namespace QrToPay.Api.Features.SkiResorts.Resorts;

public class GetSkiResortsRequestModelValidator : AbstractValidator<GetSkiResortsRequestModel>
{
    public GetSkiResortsRequestModelValidator()
    {
        RuleFor(x => x.CityName).NotEmpty().WithMessage("CityName jest wymagane.");
    }
}