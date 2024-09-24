using FluentValidation;

namespace QrToPay.Api.Features.Tickets.Active;

public class GetActiveTicketsRequestModelValidator : AbstractValidator<GetActiveTicketsRequestModel>
{
    public GetActiveTicketsRequestModelValidator()
    {
    }
}