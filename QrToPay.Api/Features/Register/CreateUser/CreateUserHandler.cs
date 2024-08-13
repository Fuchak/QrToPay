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
        User? existingUser = null;

        if (!string.IsNullOrEmpty(request.Email))
        {
            existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        }
        else if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            existingUser = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        }

        if (existingUser != null)
        {
            if (existingUser.IsVerified)
            {
                return Result<CreateUserDto>.Failure("Użytkownik z podanym e-mailem lub numerem telefonu już istnieje.");
            }

            existingUser.PasswordHash = AuthenticationHelper.HashPassword(request.PasswordHash);
            existingUser.VerificationCode = AuthenticationHelper.GenerateVerificationCode();
            existingUser.IsVerified = false;
            existingUser.UpdatedAt = DateTime.Now;
        }
        else
        {
            var newUser = new User
            {
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = AuthenticationHelper.HashPassword(request.PasswordHash),
                VerificationCode = AuthenticationHelper.GenerateVerificationCode(),
                IsVerified = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Users.Add(newUser);
        }

        await _context.SaveChangesAsync();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email || u.PhoneNumber == request.PhoneNumber);

        if (user == null)
        {
            return Result<CreateUserDto>.Failure("Błąd wewnętrzny serwera.");
        }
        var emailOrPhone = user.Email ?? user.PhoneNumber;
        var result = new CreateUserDto { VerificationCode = user.VerificationCode!, EmailOrPhone = emailOrPhone! };
        return Result<CreateUserDto>.Success(result);
    }
}