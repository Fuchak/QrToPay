using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Features.Tickets.History;

public class GetTicketHistoryHandler : IRequestHandler<GetTicketHistoryRequestModel, Result<List<TicketHistoryDto>>>
{
    private readonly QrToPayDbContext _context;

    public GetTicketHistoryHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<TicketHistoryDto>>> Handle(GetTicketHistoryRequestModel request, CancellationToken cancellationToken)
    {
        try
        {
            var ticketHistory = await _context.TicketHistories
                .Where(th => th.UserId == request.UserId)
                .OrderByDescending(th => th.PurchaseDate)
                .Select(th => new TicketHistoryDto
                {
                    Date = th.PurchaseDate.ToString("yyyy-MM-dd"),
                    Type = (ServiceType)th.Service.ServiceType,
                    Name = th.Service.ServiceName,
                    TotalPrice = th.TotalPrice
                })
                .ToListAsync(cancellationToken);

            return Result<List<TicketHistoryDto>>.Success(ticketHistory);
        }
        catch (Exception ex)
        {
            return Result<List<TicketHistoryDto>>.Failure($"Internal server error: {ex.Message}");
        }
    }
}