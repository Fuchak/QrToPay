﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Helpers;

namespace QrToPay.Api.Features.Settings.Password;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequestModel, Result<string>>
{
    private readonly QrToPayDbContext _context;

    public ChangePasswordHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<string>> Handle(ChangePasswordRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);

            if (user is null)
            {
                return Result<string>.Failure("Użytkownik z podanym identyfikatorem nie istnieje.");
            }

            if (!AuthenticationHelper.VerifyPassword(request.OldPassword, user.PasswordHash))
            {
                return Result<string>.Failure("Nieprawidłowe stare hasło.");
            }

            user.PasswordHash = AuthenticationHelper.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success("Hasło zostało zmienione.");
        });
    }
}