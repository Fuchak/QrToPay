using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Features.Tickets.Active;

public class GetActiveTicketsHandler : IRequestHandler<GetActiveTicketsRequestModel, Result<IEnumerable<ActiveTicketDto>>>
{
    private readonly QrToPayDbContext _context;

    public GetActiveTicketsHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<IEnumerable<ActiveTicketDto>>> Handle(GetActiveTicketsRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            IEnumerable<UserTicket> activeTickets = await _context.UserTickets
                            .Where(t => t.UserId == request.UserId && t.IsActive)
                            .Include(t => t.Service)
                            .ToListAsync(cancellationToken);

            if (!activeTickets.Any())
            {
                return Result<IEnumerable<ActiveTicketDto>>.Failure("Brak aktywnych biletów dla tego użytkownika.", ErrorType.NotFound);
            }

            IEnumerable<ActiveTicketDto> response = activeTickets.Select(t => new ActiveTicketDto
            {
                UserTicketId = t.UserTicketId,
                UserId = t.UserId,
                ServiceId = t.ServiceId,
                EntityType = (ServiceType)t.Service.ServiceType,
                EntityName = t.Service.ServiceName,
                CityName = t.Service.CityName,
                QrCode = t.Token?.ToString(),
                Price = t.TotalPrice,
                Points = t.RemainingTokens,
                IsActive = t.IsActive
            });

            return Result<IEnumerable<ActiveTicketDto>>.Success(response);
        });
    }
}
