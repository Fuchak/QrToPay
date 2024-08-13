using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Scan.Purchase
{
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
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);
                if (user == null)
                {
                    return Result<string>.Failure("Użytkownik nie został znaleziony.");
                }

                if (user.AccountBalance < request.Price)
                {
                    return Result<string>.Failure("Niewystarczające środki na koncie.");
                }

                var entity = await _context.Entities
                    .FirstOrDefaultAsync(e => e.EntityName == request.Type, cancellationToken);

                if (entity == null)
                {
                    return Result<string>.Failure("Atrakcja nie została znaleziona.");
                }

                user.AccountBalance -= request.Price;

                TicketHistory purchase = new()
                {
                    UserId = request.UserId,
                    EntityId = entity.EntityId,
                    PurchaseDate = DateTime.Now,
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
}