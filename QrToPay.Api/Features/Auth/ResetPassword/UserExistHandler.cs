using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Auth.ResetPassword
{
    public class UserExistHandler : IRequestHandler<UserExistRequestModel, Result<string>>
    {
        private readonly QrToPayDbContext _context;

        public UserExistHandler(QrToPayDbContext context)
        {
            _context = context;
        }

        public async Task<Result<string>> Handle(UserExistRequestModel request, CancellationToken cancellationToken)
        {
            User? user = null;

            switch (request.ChangeType)
            {
                case ChangeType.Email:
                    user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Contact, cancellationToken);
                    break;

                case ChangeType.Phone:
                    user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.Contact, cancellationToken);
                    break;

                default:
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

            var verificationCode = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = verificationCode;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<string>.Success(verificationCode);
        }
    }
}