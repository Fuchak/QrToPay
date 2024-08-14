using FluentValidation;

namespace QrToPay.Api.Features.SkiSlopes.Slopes;

public class GetSkiSlopesRequestModelValidator : AbstractValidator<GetSkiSlopesRequestModel>
{
    public GetSkiSlopesRequestModelValidator()
    {
        RuleFor(x => x.EntityId).NotEmpty().WithMessage("EntityId jest wymagany.");
    }
}