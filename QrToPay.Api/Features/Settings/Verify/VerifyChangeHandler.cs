using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Services;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Features.Settings.Verify;

public class VerifyChangeHandler : IRequestHandler<VerifyRequestModel, Result<string>>
{
    private readonly QrToPayDbContext _context;
    private readonly VerificationStorageService _verificationStorageService;

    public VerifyChangeHandler(QrToPayDbContext context, VerificationStorageService verificationStorageService)
    {
        _context = context;
        _verificationStorageService = verificationStorageService;
    }

    public async Task<Result<string>> Handle(VerifyRequestModel request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified, cancellationToken);

        if (user == null)
        {
            return Result<string>.Failure("Użytkownik z podanym identyfikatorem nie istnieje lub nie został zweryfikowany.");
        }

        if (user.VerificationCode != request.VerificationCode)
        {
            return Result<string>.Failure("Nieprawidłowy kod weryfikacyjny.");
        }

        if (request.ChangeType == ChangeType.Email)
        {
            if (!_verificationStorageService.TryGetEmailVerification(request.UserId, out var newEmail))
            {
                return Result<string>.Failure("Kod weryfikacyjny wygasł lub jest nieprawidłowy.");
            }

            user.Email = newEmail;
            _verificationStorageService.RemoveEmailVerification(request.UserId);
        }
        else if (request.ChangeType == ChangeType.Phone)
        {
            if (!_verificationStorageService.TryGetPhoneVerification(request.UserId, out var newPhone))
            {
                return Result<string>.Failure("Kod weryfikacyjny wygasł lub jest nieprawidłowy.");
            }

            user.PhoneNumber = newPhone;
            _verificationStorageService.RemovePhoneVerification(request.UserId);
        }

        user.VerificationCode = null;
        user.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<string>.Success("Zmiana została zweryfikowana.");
    }
}