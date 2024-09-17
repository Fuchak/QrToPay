using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Auth.CheckAccount;

public class CheckAccountHandler : IRequestHandler<CheckAccountRequestModel, Result<CheckAccountDto>>
{
    private readonly QrToPayDbContext _context;

    public CheckAccountHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CheckAccountDto>> Handle(CheckAccountRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .Where(u => (request.ChangeType == ChangeType.Email && u.Email == request.Contact) ||
                            (request.ChangeType == ChangeType.Phone && u.PhoneNumber == request.Contact))
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return Result<CheckAccountDto>.Failure($"Użytkownik z podanym {(request.ChangeType == ChangeType.Email ? "e-mailem" : "numerem telefonu")} nie istnieje.", ErrorType.NotFound);
            }

            if (!user.IsVerified)
            {
                return Result<CheckAccountDto>.Failure("Konto użytkownika nie zostało potwierdzone.", ErrorType.NotVerified);
            }

            string verificationCode = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = verificationCode;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<CheckAccountDto>.Success(new() { VerificationCode = user.VerificationCode});
        });
    }
}