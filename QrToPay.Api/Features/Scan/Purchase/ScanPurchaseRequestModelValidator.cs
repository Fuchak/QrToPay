using FluentValidation;

namespace QrToPay.Api.Features.Scan.Purchase
{
    public class ScanPurchaseRequestModelValidator : AbstractValidator<ScanPurchaseRequestModel>
    {
        public ScanPurchaseRequestModelValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Identyfikator użytkownika jest wymagany.");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Typ jest wymagany.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Cena musi być większa niż 0.");
        }
    }
}