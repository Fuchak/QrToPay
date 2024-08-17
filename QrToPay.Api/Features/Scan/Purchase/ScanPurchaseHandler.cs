using MediatR;
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
        try
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);
            if (user == null)
            {
                return Result<string>.Failure("Użytkownik nie został znaleziony.");
            }

            if (user.AccountBalance < request.Price)
            {
                return Result<string>.Failure("Niewystarczające środki na koncie.");
            }

            user.AccountBalance -= request.Price;

            TicketHistory purchase = new()
            {
                UserId = request.UserId,
                ServiceId = request.ServiceId,
                //Gdzie attraction name? do historii
                PurchaseDate = DateTime.UtcNow,
                Quantity = 1,
                TotalPrice = request.Price
            };

            _context.TicketHistories.Add(purchase);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success("Zakup został dodany do historii.");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure($"Wystąpił błąd serwera: {ex.Message}");
        }
    }
}