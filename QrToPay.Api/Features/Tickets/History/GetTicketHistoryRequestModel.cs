using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Tickets.History;

public class GetTicketHistoryRequestModel : IRequest<Result<List<TicketHistoryDto>>>
{
    public required int UserId { get; init; }
}