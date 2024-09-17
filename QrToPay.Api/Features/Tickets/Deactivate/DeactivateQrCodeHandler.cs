using MediatR;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace QrToPay.Api.Features.Tickets.Deactivate;
public class DeactivateQrCodeHandler : IRequestHandler<DeactivateQrCodeRequestModel, Result<SuccesMessageDto>>
{
    private readonly QrToPayDbContext _context;

    public DeactivateQrCodeHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SuccesMessageDto>> Handle(DeactivateQrCodeRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            UserTicket? response = await _context.UserTickets
                            .Where(t => t.Token == request.Token && t.UserId == request.UserID)
                            .FirstOrDefaultAsync(cancellationToken);

            if(response is null)
            {
                return Result<SuccesMessageDto>.Failure("Nie znaleziono biletu.", ErrorType.NotFound);
            }

            response.QrCodeIsActive = false; // Deaktywacja kodu
            await _context.SaveChangesAsync(cancellationToken);

            return Result<SuccesMessageDto>.Success(new() { IsSuccessful = true });
        });
    }
}
