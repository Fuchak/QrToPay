using FluentValidation;

namespace QrToPay.Api.Features.Settings.Verify;

public class VerifyRequestModelValidator : AbstractValidator<VerifyRequestModel>
{
    public VerifyRequestModelValidator()
    {
        RuleFor(x => x.VerificationCode)
            .NotEmpty()
            .WithMessage("Kod weryfikacyjny jest wymagany.");

        RuleFor(x => x.ChangeType)
            .IsInEnum()
            .WithMessage("Nieprawidłowy typ zmiany.");
    }
}