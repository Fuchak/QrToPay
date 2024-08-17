using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
using System.Data;

namespace QrToPay.Api.Features.Tickets.Activate;

public class ActivateQrCodeHandler : IRequestHandler<ActivateQrCodeRequestModel, Result<bool>>
{
    private readonly QrToPayDbContext _context;

    public ActivateQrCodeHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(ActivateQrCodeRequestModel request, CancellationToken cancellationToken)
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

            // Sprawdzenie, czy bilet jest już aktywny i czy czas aktywności wygasł
            if (ticket.QrCodeIsActive == true && ticket.QrCodeGeneratedAt.HasValue &&
                (DateTime.UtcNow - ticket.QrCodeGeneratedAt.Value).TotalMinutes <= 5)
            {
                return Result<bool>.Success(true);
            }

            // Aktualizacja biletu, aby ustawić nowy czas aktywacji
            ticket.QrCodeIsActive = true;
            ticket.QrCodeGeneratedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Wewnętrzny błąd serwera: {ex.Message}");
        }
    }
}