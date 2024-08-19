using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Services;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Features.Settings.Verify;

public class VerifyHandler : IRequestHandler<VerifyRequestModel, Result<string>>
{
    private readonly QrToPayDbContext _context;
    private readonly VerificationStorageService _verificationStorageService;

    public VerifyHandler(QrToPayDbContext context, VerificationStorageService verificationStorageService)
    {
        _context = context;
        _verificationStorageService = verificationStorageService;
    }

    public async Task<Result<string>> Handle(VerifyRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User response = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified, cancellationToken) 
                ?? throw new Exception("Użytkownik z podanym identyfikatorem nie istnieje lub nie został zweryfikowany.");

            if (response.VerificationCode != request.VerificationCode)
            {
                throw new Exception("Nieprawidłowy kod weryfikacyjny.");
            }


            if (request.ChangeType == ChangeType.Email)
            {
                if (!_verificationStorageService.TryGetEmailVerification(request.UserId, out var newEmail))
                {
                    throw new Exception("Kod weryfikacyjny wygasł lub jest nieprawidłowy.");
                }

                response.Email = newEmail;
                _verificationStorageService.RemoveEmailVerification(request.UserId);
            }
            else if (request.ChangeType == ChangeType.Phone)
            {
                if (!_verificationStorageService.TryGetPhoneVerification(request.UserId, out var newPhone))
                {
                    throw new Exception("Kod weryfikacyjny wygasł lub jest nieprawidłowy.");
                }

                response.PhoneNumber = newPhone;
                _verificationStorageService.RemovePhoneVerification(request.UserId);
            }

            response.VerificationCode = null;
            response.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return "Zmiana została zweryfikowana.";
        });
    }
}