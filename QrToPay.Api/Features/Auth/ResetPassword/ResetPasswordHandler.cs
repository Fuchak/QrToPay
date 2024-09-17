using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Auth.ResetPassword;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordRequestModel, Result<SuccesMessageDto>>
{
    private readonly QrToPayDbContext _context;

    public ResetPasswordHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<SuccesMessageDto>> Handle(ResetPasswordRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone, cancellationToken);

            if (user is null)
            {
                return Result<SuccesMessageDto>.Failure("Użytkownik z podanym e-mailem lub numerem telefonu nie istnieje.",ErrorType.NotFound);
            }

            if (user.VerificationCode != request.VerificationCode)
            {
                return Result<SuccesMessageDto>.Failure("Nieprawidłowy kod weryfikacyjny.",ErrorType.BadRequest);
            }

            user.PasswordHash = AuthenticationHelper.HashPassword(request.NewPassword);
            user.VerificationCode = null;  // Resetowanie kodu weryfikacyjnego
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<SuccesMessageDto>.Success(new() { Message = "Hasło zostało pomyślnie zaktualizowane." });
        });
    }
}