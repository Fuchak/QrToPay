using FluentValidation;

namespace QrToPay.Api.Features.Settings.EmailPhone;

public class ChangeEmailPhoneRequestModelValidator : AbstractValidator<ChangeEmailPhoneRequestModel>
{
    public ChangeEmailPhoneRequestModelValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Identyfikator użytkownika jest wymagany.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Hasło jest wymagane.");
        RuleFor(x => x.ChangeType).IsInEnum().WithMessage("Nieprawidłowy typ zmiany.");
        RuleFor(x => x.NewValue).NotEmpty().WithMessage("Nowa wartość jest wymagana.");
    }
}