using FluentValidation;

namespace QrToPay.Api.Features.Cities;

public class GetCitiesRequestModelValidator : AbstractValidator<GetCitiesRequestModel>
{
    public GetCitiesRequestModelValidator()
    {
        RuleFor(x => x.EntityType)
            .IsInEnum()
            .WithMessage("Nieprawidłowy typ encji.");
    }
}