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
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .Where(u => (request.ChangeType == ChangeType.Email && u.Email == request.Contact) ||
                            (request.ChangeType == ChangeType.Phone && u.PhoneNumber == request.Contact))
                .FirstOrDefaultAsync(cancellationToken) 
                ?? throw new Exception($"Użytkownik z podanym {(request.ChangeType == ChangeType.Email ? "e-mailem" : "numerem telefonu")} nie istnieje.");

            if (!user.IsVerified)
            {
                throw new Exception("Konto użytkownika nie zostało potwierdzone.");
            }

            string response = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = response;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        });
    }
}