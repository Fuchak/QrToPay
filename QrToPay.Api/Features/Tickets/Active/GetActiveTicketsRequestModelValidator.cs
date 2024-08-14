using FluentValidation;

namespace QrToPay.Api.Features.Tickets.Active;

public class GetActiveTicketsRequestModelValidator : AbstractValidator<GetActiveTicketsRequestModel>
{
    public GetActiveTicketsRequestModelValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Identyfikator użytkownika musi być większy niż 0.");
    }
}