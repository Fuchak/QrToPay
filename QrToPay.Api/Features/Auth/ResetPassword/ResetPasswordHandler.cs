using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Auth.ResetPassword;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequestModel, Result<string>>
{
    private readonly QrToPayDbContext _context;

    public ResetPasswordHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<string>> Handle(ResetPasswordRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone, cancellationToken) 
                ?? throw new Exception("Użytkownik z podanym e-mailem lub numerem telefonu nie istnieje.");

            if (user.VerificationCode != request.VerificationCode)
            {
                throw new Exception("Nieprawidłowy kod weryfikacyjny.");
            }

            user.PasswordHash = AuthenticationHelper.HashPassword(request.NewPassword);
            user.VerificationCode = null;  // Resetowanie kodu weryfikacyjnego
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return "Hasło zostało pomyślnie zaktualizowane.";
        });
    }
}