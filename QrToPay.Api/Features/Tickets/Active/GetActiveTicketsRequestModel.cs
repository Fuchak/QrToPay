using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Tickets.Active;

public class GetActiveTicketsRequestModel : IRequest<Result<List<TicketDto>>>
{
    public required int UserId { get; init; }
}