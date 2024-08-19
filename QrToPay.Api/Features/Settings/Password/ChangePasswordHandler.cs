using MediatR;
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
                .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken) 
                ?? throw new Exception("Użytkownik z podanym identyfikatorem nie istnieje.");

            if (!AuthenticationHelper.VerifyPassword(request.OldPassword, user.PasswordHash))
            {
                throw new Exception("Nieprawidłowe stare hasło.");
            }

            user.PasswordHash = AuthenticationHelper.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return "Hasło zostało zmienione.";
        });
    }
}