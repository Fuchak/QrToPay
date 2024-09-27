using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Extensions;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Services;

namespace QrToPay.Api.Features.Tickets.History;

public class GetTicketHistoryHandler : IRequestHandler<GetTicketHistoryRequestModel, Result<IEnumerable<TicketHistoryDto>>>
{
    private readonly QrToPayDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public GetTicketHistoryHandler(QrToPayDbContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<IEnumerable<TicketHistoryDto>>> Handle(GetTicketHistoryRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            var skip = (request.PageNumber - 1) * request.PageSize;
            IEnumerable<TicketHistoryDto> response = await _context.TicketHistories
                .Where(th => th.UserId == _currentUserService.UserId)
                .OrderByDescending(th => th.PurchaseDate)
                .Skip(skip)
                .Take(request.PageSize)
                .Select(th => new TicketHistoryDto
                {
                    Date = th.PurchaseDate.ToString("yyyy-MM-dd"),
                    Type = th.Service.ServiceType.ToEnum<ServiceType>(),
                    Name = th.Service.ServiceName,
                    TotalPrice = th.TotalPrice
                })
                .ToListAsync(cancellationToken);

            if (!response.Any())
            {
                return Result<IEnumerable<TicketHistoryDto>>.Failure("Brak biletów w historii dla tego użytkownika.", ErrorType.NotFound);
            }

            return Result<IEnumerable<TicketHistoryDto>>.Success(response);
        });
    }
}