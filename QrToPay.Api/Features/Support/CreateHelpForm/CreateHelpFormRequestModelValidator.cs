using FluentValidation;

namespace QrToPay.Api.Features.Support.CreateHelpForm;

public class CreateHelpFormRequestModelValidator : AbstractValidator<CreateHelpFormRequestModel>
{
    public CreateHelpFormRequestModelValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Imie i nazwisko jest wymagane.");
        RuleFor(x => x.UserEmail).NotEmpty().EmailAddress().WithMessage("Prawidłowy email jest wymagany.");
        RuleFor(x => x.Subject).NotEmpty().WithMessage("Temat jest wymagany.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Opis jest wymagany.");
        RuleFor(x => x.Status).NotEmpty().WithMessage("Status jest wymagany.");
    }
}