using FluentValidation;
using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Features.Auth.CheckAccount;

public class CheckAccountRequestModelValidator : AbstractValidator<CheckAccountRequestModel>
{
    public CheckAccountRequestModelValidator()
    {
        RuleFor(x => x.Contact)
            .NotEmpty().WithMessage("Kontakt jest wymagany.")
            .EmailAddress().When(x => x.ChangeType == ChangeType.Email).WithMessage("Nieprawidłowy adres email.")
            .Matches(@"^\d{9,15}$").When(x => x.ChangeType == ChangeType.Phone).WithMessage("Nieprawidłowy numer telefonu.");
        //TODO do zmiany to bo helper na froncie walidacje ma dobrą
        //TODO do zmiany changetype.email na 0 lub 1 wtedy będzie to dobrze działało na swaggerze
    }
}