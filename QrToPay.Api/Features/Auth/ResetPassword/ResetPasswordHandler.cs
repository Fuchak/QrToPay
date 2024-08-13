using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Auth.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequestModel, Result<string>>
    {
        private readonly QrToPayDbContext _context;

        public ResetPasswordHandler(QrToPayDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> Handle(ResetPasswordRequestModel request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.NewPassword))
            {
                return Result<string>.Failure("Nowe hasło jest wymagane.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone, cancellationToken);

            if (user == null)
            {
                return Result<string>.Failure("Użytkownik z podanym e-mailem lub numerem telefonu nie istnieje.");
            }

            if (user.VerificationCode != request.VerificationCode)
            {
                return Result<string>.Failure("Nieprawidłowy kod weryfikacyjny.");
            }

            // Aktualizacja hasła
            user.PasswordHash = AuthenticationHelper.HashPassword(request.NewPassword);
            user.VerificationCode = null;  // Resetowanie kodu weryfikacyjnego
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success("Hasło zostało pomyślnie zaktualizowane.");
        }
    }
}