using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Scan.Purchase;

public class ScanPurchaseHandler : IRequestHandler<ScanPurchaseRequestModel, Result<SuccesMessageDto>>
{
    private readonly QrToPayDbContext _context;

    public ScanPurchaseHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SuccesMessageDto>> Handle(ScanPurchaseRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? response = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);


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
                UserId = request.UserId,
                ServiceId = request.ServiceId,
                //Dodać do bazy kolumnę attraction name do historii biletów
                //AttractionName = request.ServiceName,
                PurchaseDate = DateTime.UtcNow,
                Quantity = 1,
                TotalPrice = request.Price
            };

            _context.TicketHistories.Add(ticketHistoryRequest);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<SuccesMessageDto>.Success(new() { Message = "Zakup został dodany do historii." });
        });
    }

}