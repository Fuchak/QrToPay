using FluentValidation;

namespace QrToPay.Api.Features.Register.VerifyCreateUser;

public class VerifyCreateUserRequestModelValidator : AbstractValidator<VerifyCreateUserRequestModel>
{
    public VerifyCreateUserRequestModelValidator()
    {
        RuleFor(x => x.EmailOrPhone).NotEmpty().WithMessage("Email lub numer telefonu jest wymagany.");
        RuleFor(x => x.VerificationCode).NotEmpty().WithMessage("Kod weryfikacyjny jest wymagany.");
    }
}