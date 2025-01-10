using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Services;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Scan.Purchase;

public class ScanPurchaseHandler : IRequestHandler<ScanPurchaseRequestModel, Result<SuccesMessageDto>>
{
    private readonly QrToPayDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public ScanPurchaseHandler(QrToPayDbContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<SuccesMessageDto>> Handle(ScanPurchaseRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? response = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == _currentUserService.UserId, cancellationToken);


            if (response is null)
            {
                return Result<SuccesMessageDto>.Failure("Użytkownik nie został znaleziony.", ErrorType.NotFound);
            }

            if (response.AccountBalance < request.Price)
            {
                return Result<SuccesMessageDto>.Failure("Niewystarczające środki na koncie.", ErrorType.BadRequest);
            }

            response.AccountBalance -= request.Price;

            TicketHistory ticketHistoryRequest = new()
            {
                UserId = _currentUserService.UserId,
                ServiceId = request.ServiceId,
                AttractionName = request.ServiceName,
                CreatedAt = DateTime.UtcNow,
                Quantity = 1,
                TotalPrice = request.Price
            };

            _context.TicketHistories.Add(ticketHistoryRequest);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<SuccesMessageDto>.Success(new() { Message = "Zakup został dodany do historii." });
        });
    }

}