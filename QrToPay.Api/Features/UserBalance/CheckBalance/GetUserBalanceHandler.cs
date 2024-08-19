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
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            UserBalanceDto? response = await _context.Users
                .Where(u => u.UserId == request.UserId)
                .Select(u => new UserBalanceDto { AccountBalance = u.AccountBalance })
                .FirstOrDefaultAsync(cancellationToken) 
                ?? throw new Exception("Użytkownik nieodnaleziony.");

            return response;
        });
    }
}