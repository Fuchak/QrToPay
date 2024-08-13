using FluentValidation;

namespace QrToPay.Api.Features.Auth.ResetPassword
{
    public class ResetPasswordRequestModelValidator : AbstractValidator<ResetPasswordRequestModel>
    {
        public ResetPasswordRequestModelValidator()
        {
            RuleFor(x => x.EmailOrPhone).NotEmpty().WithMessage("Email lub numer telefonu jest wymagany.");
            RuleFor(x => x.VerificationCode).NotEmpty().WithMessage("Kod weryfikacyjny jest wymagany.");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Nowe hasło jest wymagane.");
        }
    }
}