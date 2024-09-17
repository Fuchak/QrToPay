﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Common.Services;

namespace QrToPay.Api.Features.Settings.EmailPhone;

public class ChangeEmailPhoneRequestHandler : IRequestHandler<ChangeEmailPhoneRequestModel, Result<SuccesMessageDto>>
{
    private readonly QrToPayDbContext _context;
    private readonly VerificationStorageService _verificationStorageService;

    public ChangeEmailPhoneRequestHandler(QrToPayDbContext context, VerificationStorageService verificationStorageService)
    {
        _context = context;
        _verificationStorageService = verificationStorageService;
    }

    public async Task<Result<SuccesMessageDto>> Handle(ChangeEmailPhoneRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified, cancellationToken);

            if(user is null)
            {
                return Result<SuccesMessageDto>.Failure("Użytkownik z podanym identyfikatorem nie istnieje.", ErrorType.NotFound);
            }

            if (!AuthenticationHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Result<SuccesMessageDto>.Failure("Nieprawidłowe hasło.", ErrorType.BadRequest);
            }

            //User? existingUser = null;

            if (request.ChangeType == ChangeType.Email)
            {
                User? existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == request.NewValue, cancellationToken);

                if(existingUser != null)
                {
                    return Result<SuccesMessageDto>.Failure($"Użytkownik z takim adresem email już istnieje.", ErrorType.BadRequest);
                }

                _verificationStorageService.StoreEmailVerification(user.UserId, request.NewValue);
            }
            else if (request.ChangeType == ChangeType.Phone)
            {
                User? existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == request.NewValue, cancellationToken);

                if (existingUser != null)
                {
                    return Result<SuccesMessageDto>.Failure($"Użytkownik z takim numerem telefonu już istnieje.", ErrorType.BadRequest);
                }

                _verificationStorageService.StorePhoneVerification(user.UserId, request.NewValue);
            }
            else
            {
                return Result<SuccesMessageDto>.Failure("Nieprawidłowy typ zmiany.", ErrorType.BadRequest);
            }

            string verificationCode = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = verificationCode;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<SuccesMessageDto>.Success(new() { Message = verificationCode });
        });
    }

}