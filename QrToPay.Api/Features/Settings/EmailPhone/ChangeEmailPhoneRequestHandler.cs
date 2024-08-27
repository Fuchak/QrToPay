using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Common.Services;

namespace QrToPay.Api.Features.Settings.EmailPhone;

public class ChangeEmailPhoneRequestHandler : IRequestHandler<ChangeEmailPhoneRequestModel, Result<string>>
{
    private readonly QrToPayDbContext _context;
    private readonly VerificationStorageService _verificationStorageService;

    public ChangeEmailPhoneRequestHandler(QrToPayDbContext context, VerificationStorageService verificationStorageService)
    {
        _context = context;
        _verificationStorageService = verificationStorageService;
    }

    public async Task<Result<string>> Handle(ChangeEmailPhoneRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified, cancellationToken);

            if(user is null)
            {
                return Result<string>.Failure("Użytkownik z podanym identyfikatorem nie istnieje lub nie został zweryfikowany.");
            }

            if (!AuthenticationHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Result<string>.Failure("Nieprawidłowe hasło.");
            }

            //User? existingUser = null;

            if (request.ChangeType == ChangeType.Email)
            {
                User? existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.NewValue, cancellationToken);

                if(existingUser != null)
                {
                    return Result<string>.Failure($"Użytkownik z takim adresem email już istnieje.");
                }

                _verificationStorageService.StoreEmailVerification(user.UserId, request.NewValue);
            }
            else if (request.ChangeType == ChangeType.Phone)
            {
                User? existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == request.NewValue, cancellationToken);

                if (existingUser != null)
                {
                    return Result<string>.Failure($"Użytkownik z takim numerem telefonu już istnieje.");
                }

                _verificationStorageService.StorePhoneVerification(user.UserId, request.NewValue);
            }
            else
            {
                return Result<string>.Failure("Nieprawidłowy typ zmiany.");
            }

            string response = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = response;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(response);
        });
    }

}