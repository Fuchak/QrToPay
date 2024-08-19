using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Register.VerifyCreateUser;

public class VerifyCreateUserHandler : IRequestHandler<VerifyCreateUserRequestModel, Result<string>>
{
    private readonly QrToPayDbContext _context;

    public VerifyCreateUserHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<string>> Handle(VerifyCreateUserRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User response = await _context.Users
                            .FirstOrDefaultAsync(u => (u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone) && u.VerificationCode == request.VerificationCode, cancellationToken)
                            ?? throw new Exception("Nieprawidłowy kod weryfikacyjny.");

            response.IsVerified = true;
            response.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return "Użytkownik został zweryfikowany.";
        });
    }
}