using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Auth.CheckAccount;

public class CheckAccountHandler : IRequestHandler<CheckAccountRequestModel, Result<string>>
{
    private readonly QrToPayDbContext _context;

    public CheckAccountHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<string>> Handle(CheckAccountRequestModel request, CancellationToken cancellationToken)
    {
        User? user = null;

        if (request.ChangeType == ChangeType.Email)
        {
            user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Contact, cancellationToken);
        }
        else if (request.ChangeType == ChangeType.Phone)
        {
            user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.Contact, cancellationToken);
        }
        else
        {
            return Result<string>.Failure("Nieprawidłowy typ zmiany.");
        }

        if (user == null)
        {
            return Result<string>.Failure($"Użytkownik z podanym {(request.ChangeType == ChangeType.Email ? "e-mailem" : "numerem telefonu")} nie istnieje.");
        }

        if (!user.IsVerified)
        {
            return Result<string>.Failure("Konto użytkownika nie zostało potwierdzone.");
        }

        string verificationCode = AuthenticationHelper.GenerateVerificationCode();
        user.VerificationCode = verificationCode;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(verificationCode);
    }
}