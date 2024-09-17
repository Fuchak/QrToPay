using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;

namespace QrToPay.Api.Features.Auth.Login;

public class LoginHandler : IRequestHandler<LoginRequestModel, Result<LoginDto>>
{
    private readonly QrToPayDbContext _context;

    public LoginHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<LoginDto>> Handle(LoginRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .Where(u => (!string.IsNullOrEmpty(request.Email) && u.Email == request.Email) ||
                            (!string.IsNullOrEmpty(request.PhoneNumber) && u.PhoneNumber == request.PhoneNumber))
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.PasswordHash, user.PasswordHash))
            {
                return Result<LoginDto>.Failure("Nieprawidłowe dane logowania.", ErrorType.BadRequest);
            }

            if (!user.IsVerified)
            {
                return Result<LoginDto>.Failure("Konto nie aktywowane.", ErrorType.NotVerified);
            }

            if (user.IsDeleted)
            {
                return Result<LoginDto>.Failure("Konto zablokowane.", ErrorType.Unauthorized);
            }

            LoginDto response = new()
            {
                UserId = user.UserId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AccountBalance = user.AccountBalance
                //Podmianka email phone i balance na token jwt?
            };

            return Result<LoginDto>.Success(response);
        });
    }
}