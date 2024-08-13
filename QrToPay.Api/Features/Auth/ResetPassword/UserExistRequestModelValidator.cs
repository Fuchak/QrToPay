using FluentValidation;
using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Features.Auth.ResetPassword
{
    public class UserExistRequestModelValidator : AbstractValidator<UserExistRequestModel>
    {
        public UserExistRequestModelValidator()
        {
            RuleFor(x => x.Contact)
                .NotEmpty().WithMessage("Kontakt jest wymagany.")
                .EmailAddress().When(x => x.ChangeType == ChangeType.Email).WithMessage("Nieprawidłowy adres email.")
                .Matches(@"^\d{9,15}$").When(x => x.ChangeType == ChangeType.Phone).WithMessage("Nieprawidłowy numer telefonu.");
        }
    }
}