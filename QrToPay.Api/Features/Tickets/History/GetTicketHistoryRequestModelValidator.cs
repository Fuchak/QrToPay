using FluentValidation;

namespace QrToPay.Api.Features.Tickets.History;

public class GetTicketHistoryRequestModelValidator : AbstractValidator<GetTicketHistoryRequestModel>
{
    public GetTicketHistoryRequestModelValidator()
    {
    }
}