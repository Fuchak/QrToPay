using FluentValidation;

namespace QrToPay.Api.Features.Tickets.Purchase;
public class PurchaseTicketRequestModelValidator : AbstractValidator<PurchaseTicketRequestModel>
{
    public PurchaseTicketRequestModelValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Identyfikator użytkownika musi być większy niż 0.");
        RuleFor(x => x.ServiceId).NotEmpty().WithMessage("Identyfikator jednostki nie może być pusty.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Ilość musi być większa niż 0.");
        RuleFor(x => x.Tokens).GreaterThanOrEqualTo(0).WithMessage("Liczba tokenów musi być większa lub równa 0.");
        RuleFor(x => x.TotalPrice).GreaterThanOrEqualTo(0).WithMessage("Cena całkowita musi być większa lub równa 0.");
    }
}