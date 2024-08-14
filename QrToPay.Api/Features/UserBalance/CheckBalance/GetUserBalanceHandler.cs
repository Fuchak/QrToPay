using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.UserBalance.CheckBalance;

public class GetUserBalanceHandler : IRequestHandler<GetUserBalanceRequestModel, Result<UserBalanceDto>>
{
    private readonly QrToPayDbContext _context;

    public GetUserBalanceHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UserBalanceDto>> Handle(GetUserBalanceRequestModel request, CancellationToken cancellationToken)
    {
        var balance = await _context.Users
            .Where(u => u.UserId == request.UserId)
            .Select(u => new UserBalanceDto { AccountBalance = u.AccountBalance })
            .FirstOrDefaultAsync(cancellationToken);

        if (balance == null)
        {
            return Result<UserBalanceDto>.Failure("Użytkownik nieodnaleziony.");
        }

        return Result<UserBalanceDto>.Success(balance);
    }
}