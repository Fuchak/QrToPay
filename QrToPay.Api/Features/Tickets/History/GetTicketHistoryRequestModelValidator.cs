using FluentValidation;

namespace QrToPay.Api.Features.Tickets.History;

public class GetTicketHistoryRequestModelValidator : AbstractValidator<GetTicketHistoryRequestModel>
{
    public GetTicketHistoryRequestModelValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Identyfikator użytkownika musi być większy niż 0.");
    }
}