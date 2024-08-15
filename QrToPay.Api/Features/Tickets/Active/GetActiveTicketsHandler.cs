using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.Tickets.Active;

public class GetActiveTicketsHandler : IRequestHandler<GetActiveTicketsRequestModel, Result<List<ActiveTicketDto>>>
{
    private readonly QrToPayDbContext _context;

    public GetActiveTicketsHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<ActiveTicketDto>>> Handle(GetActiveTicketsRequestModel request, CancellationToken cancellationToken)
    {
        var activeTickets = await _context.UserTickets
            .Where(t => t.UserId == request.UserId && t.IsActive)
            .Include(t => t.Entity)
            .ToListAsync(cancellationToken);

        var ticketResponses = activeTickets.Select(t => new ActiveTicketDto
        {
            UserTicketId = t.UserTicketId,
            UserId = t.UserId,
            EntityId = t.EntityId,
            EntityType = t.Entity.EntityType,
            EntityName = t.Entity.EntityName,
            CityName = t.Entity.CityName,
            QrCode = t.Qrcode,
            Price = t.TotalPrice,
            Points = t.RemainingTokens,
            IsActive = t.IsActive
        }).ToList();

        return Result<List<ActiveTicketDto>>.Success(ticketResponses);
    }
}