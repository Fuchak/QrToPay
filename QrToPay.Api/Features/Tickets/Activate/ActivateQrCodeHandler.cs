using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
namespace QrToPay.Api.Features.Tickets.Activate;

public class ActivateQrCodeHandler : IRequestHandler<ActivateQrCodeRequestModel, Result<SuccesMessageDto>>
{
    private readonly QrToPayDbContext _context;

    public ActivateQrCodeHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SuccesMessageDto>> Handle(ActivateQrCodeRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            UserTicket? response = await _context.UserTickets
                .Where(t => t.Token == request.Token && t.UserId == request.UserID)
                .FirstOrDefaultAsync(cancellationToken);

            if (response is null)
            {
                return Result<SuccesMessageDto>.Failure("Nie znaleziono biletu.", ErrorType.NotFound);
            }

            response.QrCodeIsActive = true;
            response.QrCodeGeneratedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<SuccesMessageDto>.Success(new() { IsSuccessful = true });
        });
    }
}