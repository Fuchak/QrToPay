using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Services;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Features.Settings.Verify;

public class VerifyHandler : IRequestHandler<VerifyRequestModel, Result<SuccesMessageDto>>
{
    private readonly QrToPayDbContext _context;
    private readonly VerificationStorageService _verificationStorageService;
    private readonly CurrentUserService _currentUserService;

    public VerifyHandler(QrToPayDbContext context, VerificationStorageService verificationStorageService, CurrentUserService currentUserService)
    {
        _context = context;
        _verificationStorageService = verificationStorageService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<SuccesMessageDto>> Handle(VerifyRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? response = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == _currentUserService.UserId && u.IsVerified, cancellationToken);

            if (response is null)
            {
                   return Result<SuccesMessageDto>.Failure("Użytkownik z podanym identyfikatorem nie istnieje.", ErrorType.NotFound);
            }

            if (response.VerificationCode != request.VerificationCode)
            {
                return Result<SuccesMessageDto>.Failure("Nieprawidłowy kod weryfikacyjny.", ErrorType.BadRequest);
            }

            if (request.ChangeType == ChangeType.Email)
            {
                if (!_verificationStorageService.TryGetEmailVerification(_currentUserService.UserId, out var newEmail))
                {
                    return Result<SuccesMessageDto>.Failure("Kod weryfikacyjny wygasł lub jest nieprawidłowy.", ErrorType.BadRequest);
                }

                response.Email = newEmail;
                _verificationStorageService.RemoveEmailVerification(_currentUserService.UserId);
            }
            else if (request.ChangeType == ChangeType.Phone)
            {
                if (!_verificationStorageService.TryGetPhoneVerification(_currentUserService.UserId, out var newPhone))
                {
                    return Result<SuccesMessageDto>.Failure("Kod weryfikacyjny wygasł lub jest nieprawidłowy.", ErrorType.BadRequest);
                }

                response.PhoneNumber = newPhone;
                _verificationStorageService.RemovePhoneVerification(_currentUserService.UserId);
            }

            response.VerificationCode = null;
            response.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<SuccesMessageDto>.Success(new() { Message = "Zmiana została zweryfikowana." });
        });
    }
}