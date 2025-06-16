using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Common.Services;

namespace QrToPay.Api.Features.Settings.EmailPhone;

public class ChangeEmailPhoneRequestHandler : IRequestHandler<ChangeEmailPhoneRequestModel, Result<ChangeEmailPhoneDto>>
{
    private readonly QrToPayDbContext _context;
    private readonly VerificationStorageService _verificationStorageService;
    private readonly CurrentUserService _currentUserService;

    public ChangeEmailPhoneRequestHandler(QrToPayDbContext context, VerificationStorageService verificationStorageService, CurrentUserService currentUserService)
    {
        _context = context;
        _verificationStorageService = verificationStorageService;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ChangeEmailPhoneDto>> Handle(ChangeEmailPhoneRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == _currentUserService.UserId && u.IsVerified, cancellationToken);

            if(user is null)
            {
                return Result<ChangeEmailPhoneDto>.Failure("Użytkownik z podanym identyfikatorem nie istnieje.", ErrorType.NotFound);
            }

            if (!AuthenticationHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Result<ChangeEmailPhoneDto>.Failure("Nieprawidłowe hasło.", ErrorType.BadRequest);
            }

            //User? existingUser = null;


            //Tu można zastosować fabrykę czy jak się to zwie przekształcić te if-else w coś korzystniejszego i wydajeniejszego
            if (request.ChangeType == ChangeType.Email)
            {
                User? existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.NewValue, cancellationToken);

                if(existingUser != null)
                {
                    return Result<ChangeEmailPhoneDto>.Failure($"Użytkownik z takim adresem email już istnieje.", ErrorType.BadRequest);
                }

                _verificationStorageService.StoreEmailVerification(user.UserId, request.NewValue);
            }
            else if (request.ChangeType == ChangeType.Phone)
            {
                User? existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == request.NewValue, cancellationToken);

                if (existingUser != null)
                {
                    return Result<ChangeEmailPhoneDto>.Failure($"Użytkownik z takim numerem telefonu już istnieje.", ErrorType.BadRequest);
                }

                _verificationStorageService.StorePhoneVerification(user.UserId, request.NewValue);
            }
            else
            {
                return Result<ChangeEmailPhoneDto>.Failure("Nieprawidłowy typ zmiany.", ErrorType.BadRequest);
            }

            string verificationCode = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = verificationCode;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<ChangeEmailPhoneDto>.Success(new() { VerificationCode = verificationCode });
        });
    }

}