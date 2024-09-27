using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Register.VerifyCreateUser;

public class VerifyCreateUserHandler : IRequestHandler<VerifyCreateUserRequestModel, Result<SuccesMessageDto>>
{
    private readonly QrToPayDbContext _context;

    public VerifyCreateUserHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SuccesMessageDto>> Handle(VerifyCreateUserRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? response = await _context.Users
                            .FirstOrDefaultAsync(u => (u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone) && u.VerificationCode == request.VerificationCode, cancellationToken);

            if (response is null)
            {
                return Result<SuccesMessageDto>.Failure("Nieprawidłowy kod weryfikacyjny.", ErrorType.BadRequest);
            }

            response.IsVerified = true;
            response.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return Result<SuccesMessageDto>.Success(new() { Message = "Użytkownik został zweryfikowany." });
        });
    }
}