using FluentValidation;

namespace QrToPay.Api.Features.Tickets.Activate;

public class ActivateQrCodeRequestModelValidator : AbstractValidator<ActivateQrCodeRequestModel>
{
    public ActivateQrCodeRequestModelValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token jest wymagany.");
        RuleFor(x => x.UserID).GreaterThan(0).WithMessage("Identyfikator użytkownika musi być większy niż 0.");
    }
}