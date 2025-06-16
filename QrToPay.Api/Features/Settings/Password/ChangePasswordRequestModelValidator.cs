using FluentValidation;

namespace QrToPay.Api.Features.Settings.Password;

public class ChangePasswordRequestModelValidator : AbstractValidator<ChangePasswordRequestModel>
{
    public ChangePasswordRequestModelValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .WithMessage("Stare hasło jest wymagane.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("Nowe hasło jest wymagane.");

        RuleFor(x => x.ConfirmNewPassword)
            .Equal(x => x.NewPassword)
            .WithMessage("Hasła nie są zgodne.");
    }
}