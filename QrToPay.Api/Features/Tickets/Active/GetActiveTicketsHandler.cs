using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Services;

namespace QrToPay.Api.Features.Tickets.Active;

public class GetActiveTicketsHandler : IRequestHandler<GetActiveTicketsRequestModel, Result<IEnumerable<ActiveTicketDto>>>
{
    private readonly QrToPayDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public GetActiveTicketsHandler(QrToPayDbContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<IEnumerable<ActiveTicketDto>>> Handle(GetActiveTicketsRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            IEnumerable<UserTicket> activeTickets = await _context.UserTickets
                            .Where(t => t.UserId == _currentUserService.UserId && t.IsActive)
                            .Include(t => t.Service)
                            .ToListAsync(cancellationToken);

            if (!activeTickets.Any())
            {
                return Result<IEnumerable<ActiveTicketDto>>.Failure("Brak aktywnych biletów dla tego użytkownika.", ErrorType.NotFound);
            }

            IEnumerable<ActiveTicketDto> response = activeTickets.Select(t => new ActiveTicketDto
            {
                UserTicketId = t.UserTicketId,
                //UserId = t.UserId,
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
