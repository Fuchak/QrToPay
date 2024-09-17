﻿using MediatR;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.UserBalance.TopUp;

public class TopUpAccountHandler : IRequestHandler<TopUpRequestModel, Result<decimal>>
{
    private readonly QrToPayDbContext _context;

    public TopUpAccountHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<decimal>> Handle(TopUpRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .FindAsync([request.UserId], cancellationToken);
            if (user is null)
            {
                return Result<decimal>.Failure("Użytkownik nieodnaleziony.", ErrorType.NotFound);
            }

            user.AccountBalance += request.Amount;
            user.UpdatedAt = DateTime.UtcNow;

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<decimal>.Success(user.AccountBalance);
        });
    }
}