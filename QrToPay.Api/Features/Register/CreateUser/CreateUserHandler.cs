using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Common.Helpers;

namespace QrToPay.Api.Features.Register.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserRequestModel, Result<CreateUserDto>>
{
    private readonly QrToPayDbContext _context;

    public CreateUserHandler(QrToPayDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CreateUserDto>> Handle(CreateUserRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? existingUser = await _context.Users
                .FirstOrDefaultAsync(u =>
                    (request.Email != null && u.Email == request.Email) ||
                    (request.PhoneNumber != null && u.PhoneNumber == request.PhoneNumber),
                    cancellationToken);

            if (existingUser != null)
            {
                if (existingUser.IsVerified)
                {
                    throw new Exception("Użytkownik z podanym e-mailem lub numerem telefonu już istnieje.");
                }

                existingUser.PasswordHash = AuthenticationHelper.HashPassword(request.PasswordHash);
                existingUser.VerificationCode = AuthenticationHelper.GenerateVerificationCode();
                existingUser.IsVerified = false;
                existingUser.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                User newUser = new()
                {
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = AuthenticationHelper.HashPassword(request.PasswordHash),
                    VerificationCode = AuthenticationHelper.GenerateVerificationCode(),
                    IsVerified = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(newUser);
            }

            await _context.SaveChangesAsync(cancellationToken);

            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email || u.PhoneNumber == request.PhoneNumber, cancellationToken) 
                ?? throw new Exception("Błąd wewnętrzny serwera.");

            CreateUserDto result = new() 
            {
                VerificationCode = user.VerificationCode!, 
                EmailOrPhone = user.Email ?? user.PhoneNumber!
            };

            return result;
        });
    }
}