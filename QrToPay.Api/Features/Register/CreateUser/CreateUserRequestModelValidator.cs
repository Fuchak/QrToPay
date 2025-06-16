using FluentValidation;

namespace QrToPay.Api.Features.Register.CreateUser;

public class CreateUserRequestModelValidator : AbstractValidator<CreateUserRequestModel>
{
    public CreateUserRequestModelValidator()
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