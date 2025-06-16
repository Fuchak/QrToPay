using FluentValidation;

namespace QrToPay.Api.Features.Auth.Login;

public class LoginRequestModelValidator : AbstractValidator<LoginRequestModel>
{
    public LoginRequestModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .When(x => string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage("Email lub numer telefonu jest wymagany.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .When(x => string.IsNullOrEmpty(x.Email))
            .WithMessage("Email lub numer telefonu jest wymagany.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty()
            .WithMessage("Hasło jest wymagane.");
    }
}