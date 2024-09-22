using MediatR;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Services;

namespace QrToPay.Api.Features.UserBalance.TopUp;

public class TopUpAccountHandler : IRequestHandler<TopUpRequestModel, Result<TopUpAccountDto>>
{
    private readonly QrToPayDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public TopUpAccountHandler(QrToPayDbContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<TopUpAccountDto>> Handle(TopUpRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .FindAsync([_currentUserService.UserId], cancellationToken);
            if (user is null)
            {
                return Result<TopUpAccountDto>.Failure("Użytkownik nieodnaleziony.", ErrorType.NotFound);
            }

            user.AccountBalance += request.Amount;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<TopUpAccountDto>.Success(new() { AccountBalance = user.AccountBalance });
        });
    }
}