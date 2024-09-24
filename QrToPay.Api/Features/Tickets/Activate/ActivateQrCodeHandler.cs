using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Services;
namespace QrToPay.Api.Features.Tickets.Activate;

public class ActivateQrCodeHandler : IRequestHandler<ActivateQrCodeRequestModel, Result<SuccesMessageDto>>
{
    private readonly QrToPayDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public ActivateQrCodeHandler(QrToPayDbContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<SuccesMessageDto>> Handle(ActivateQrCodeRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            UserTicket? response = await _context.UserTickets
                .Where(t => t.Token == request.Token && t.UserId == _currentUserService.UserId)
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