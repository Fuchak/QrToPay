using MediatR;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Tickets.History;

public class GetTicketHistoryRequestModel : IRequest<Result<IEnumerable<TicketHistoryDto>>>
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
}