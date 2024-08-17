using FluentValidation;

namespace QrToPay.Api.Features.Tickets.Deactivate;

public class DeactivateQrCodeRequestModelValidator : AbstractValidator<DeactivateQrCodeRequestModel>
{
    public DeactivateQrCodeRequestModelValidator()
    {
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token jest wymagany.");
        RuleFor(x => x.UserID).GreaterThan(0).WithMessage("Identyfikator użytkownika musi być większy niż 0.");
    }
}
