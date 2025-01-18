using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Services;
using QrToPay.Api.Extensions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                            .Include(t => t.Group)
                            .ThenInclude(g => g.CompanyGroupMembers)
                            .ThenInclude(gm => gm.Service)
                            .ToListAsync(cancellationToken);

            if (!activeTickets.Any())
            {
                return Result<IEnumerable<ActiveTicketDto>>.Failure("Brak aktywnych biletów dla tego użytkownika.", ErrorType.NotFound);
            }

            IEnumerable<ActiveTicketDto> response = activeTickets.Select(ticket =>
            {
                IEnumerable<string> serviceNames = ticket.Group.CompanyGroupMembers
                    .Select(gm => $"{gm.Service.ServiceName} - {gm.Service.CityName}");

                return new ActiveTicketDto
                {
                    UserTicketId = ticket.UserTicketId,
                    GroupId = ticket.GroupId,
                    EntityNames = serviceNames,
                    Price = ticket.TotalPrice,
                    Points = ticket.RemainingTokens,
                    IsActive = ticket.IsActive,
                    QrCode = ticket.Token.ToString()!
                };
            });

            return Result<IEnumerable<ActiveTicketDto>>.Success(response);
        });
    }
}
