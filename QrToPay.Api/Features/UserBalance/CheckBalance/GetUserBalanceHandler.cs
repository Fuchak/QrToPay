using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Services;

namespace QrToPay.Api.Features.UserBalance.CheckBalance;

public class GetUserBalanceHandler : IRequestHandler<GetUserBalanceRequestModel, Result<UserBalanceDto>>
{
    private readonly QrToPayDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetUserBalanceHandler(QrToPayDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<UserBalanceDto>> Handle(GetUserBalanceRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            UserBalanceDto? response = await _context.Users
                .Where(u => u.UserId == _currentUserService.UserId)
                .Select(u => new UserBalanceDto { AccountBalance = u.AccountBalance })
                .FirstOrDefaultAsync(cancellationToken);

            if(response is null)
            {
                return Result<UserBalanceDto>.Failure("Użytkownik nieodnaleziony.",ErrorType.NotFound);
            }

            return Result<UserBalanceDto>.Success(response);
        });
    }
}