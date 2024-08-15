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
        User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified, cancellationToken);

        if (user == null)
        {
            return Result<string>.Failure("Użytkownik z podanym identyfikatorem nie istnieje lub nie został zweryfikowany.");
        }

        if (!AuthenticationHelper.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Result<string>.Failure("Nieprawidłowe hasło.");
        }

        User? existingUser = null;
        //Action<string, string> sendVerification = null;

        if (request.ChangeType == ChangeType.Email)
        {
            existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.NewValue, cancellationToken);
            _verificationStorageService.StoreEmailVerification(user.UserId, request.NewValue);
            //sendVerification = SendVerificationCodeEmail;
        }
        else if (request.ChangeType == ChangeType.Phone)
        {
            existingUser = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.NewValue, cancellationToken);
            _verificationStorageService.StorePhoneVerification(user.UserId, request.NewValue);
            //sendVerification = SendVerificationCodeSMS;
        }
        else
        {
            return Result<string>.Failure("Nieprawidłowy typ zmiany.");
        }

        if (existingUser != null)
        {
            return Result<string>.Failure($"Użytkownik z nowym {(request.ChangeType == ChangeType.Email ? "adresem email" : "numerem telefonu")} już istnieje.");
        }

        string verificationCode = AuthenticationHelper.GenerateVerificationCode();
        user.VerificationCode = verificationCode;
        user.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        //sendVerification(request.NewValue, verificationCode);

        return Result<string>.Success(verificationCode);
    }
}