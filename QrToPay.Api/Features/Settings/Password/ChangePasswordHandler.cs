using MediatR;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Models;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Common.Services;

namespace QrToPay.Api.Features.Settings.Password;

public class ChangePasswordHandler : IRequestHandler<ChangePasswordRequestModel, Result<SuccesMessageDto>>
{
    private readonly QrToPayDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public ChangePasswordHandler(QrToPayDbContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<SuccesMessageDto>> Handle(ChangePasswordRequestModel request, CancellationToken cancellationToken)
    {
        return await ResultHandler.HandleRequestAsync(async () =>
        {
            User? user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == _currentUserService.UserId, cancellationToken);

            if (user is null)
            {
                return Result<SuccesMessageDto>.Failure("Użytkownik z podanym identyfikatorem nie istnieje.", ErrorType.NotFound);
            }

            if (!AuthenticationHelper.VerifyPassword(request.OldPassword, user.PasswordHash))
            {
                return Result<SuccesMessageDto>.Failure("Nieprawidłowe stare hasło.", ErrorType.BadRequest);
            }

            user.PasswordHash = AuthenticationHelper.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Result<SuccesMessageDto>.Success(new() { Message = "Hasło zostało zmienione." });
        });
    }
}