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
        var user = await _context.Users
            .FirstOrDefaultAsync(u => (u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone) && u.VerificationCode == request.VerificationCode);

        if (user == null)
        {
            return Result<string>.Failure("Nieprawidłowy kod weryfikacyjny.");
        }

        user.IsVerified = true;
        user.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync(cancellationToken);

        return Result<string>.Success("Użytkownik został zweryfikowany.");
    }
}