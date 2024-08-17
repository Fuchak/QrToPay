using MediatR;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace QrToPay.Api.Features.Tickets.Deactivate;
public class DeactivateQrCodeHandler : IRequestHandler<DeactivateQrCodeRequestModel, Result<bool>>
{
    private readonly QrToPayDbContext _context;

    public DeactivateQrCodeHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeactivateQrCodeRequestModel request, CancellationToken cancellationToken)
    {
        try
        {
            var ticket = await _context.UserTickets
                .Where(t => t.Token == request.Token && t.UserId == request.UserID)
                .FirstOrDefaultAsync(cancellationToken);

            if (ticket == null)
            {
                return Result<bool>.Failure("Nie znaleziono biletu.");
            }

            ticket.QrCodeIsActive = false; // Deaktywacja kodu
            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Wewnętrzny błąd serwera: {ex.Message}");
        }
    }
}
