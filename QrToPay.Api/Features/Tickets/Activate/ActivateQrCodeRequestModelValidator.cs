using FluentValidation;

namespace QrToPay.Api.Features.Tickets.Activate;

public class ActivateQrCodeRequestModelValidator : AbstractValidator<ActivateQrCodeRequestModel>
{
    public ActivateQrCodeRequestModelValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token jest wymagany.");
    }
}