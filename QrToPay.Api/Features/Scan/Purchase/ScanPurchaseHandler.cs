﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Scan.Purchase;

public class ScanPurchaseHandler : IRequestHandler<ScanPurchaseRequestModel, Result<string>>
{
    private readonly QrToPayDbContext _context;

    public ScanPurchaseHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<string>> Handle(ScanPurchaseRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? response = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);


            if (response is null)
            {
                return Result<string>.Failure("Użytkownik nie został znaleziony.");
            }

            if (response.AccountBalance < request.Price)
            {
                return Result<string>.Failure("Niewystarczające środki na koncie.");
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

            return Result<string>.Success("Zakup został dodany do historii.");
        });
    }

}