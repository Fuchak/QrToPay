using FluentValidation;

namespace QrToPay.Api.Features.SkiResorts.Resorts;

public class GetSkiResortsRequestModelValidator : AbstractValidator<GetSkiResortsRequestModel>
{
    public GetSkiResortsRequestModelValidator()
    {
        RuleFor(x => x.ServiceId).NotEmpty().WithMessage("ServiceId jest wymagany.");
    }
}