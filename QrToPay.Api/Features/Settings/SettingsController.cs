using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using System.Collections.Concurrent;
using QrToPay.Api.Common.Enums;
using QrToPay.Api.Common.Helpers;
using QrToPay.Api.Features.Settings.Password;
using QrToPay.Api.Features.Settings.Verify;
using QrToPay.Api.Features.Settings.EmailPhone;

namespace QrToPay.Api.Features.Settings
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly QrToPayDbContext _context;

        public SettingsController(QrToPayDbContext context)
        {
            _context = context;
        }

        private static readonly ConcurrentDictionary<int, string> _emailVerificationStorage = new();
        private static readonly ConcurrentDictionary<int, string> _phoneVerificationStorage = new();

        [HttpPost("requestChange")]
        public async Task<IActionResult> RequestChange(ChangeRequestModel request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik z podanym identyfikatorem nie istnieje lub nie został zweryfikowany." });
            }

            if (!AuthenticationHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return BadRequest(new { Message = "Nieprawidłowe hasło." });
            }

            User? existingUser = null;
            ConcurrentDictionary<int, string> storage;
            //Action<string, string> sendVerification = null;

            if (request.ChangeType == ChangeType.Email)
            {
                existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.NewValue);
                storage = _emailVerificationStorage;
                //sendVerification = SendVerificationCodeEmail;
            }
            else if (request.ChangeType == ChangeType.Phone)
            {
                existingUser = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.NewValue);
                storage = _phoneVerificationStorage;
                //sendVerification = SendVerificationCodeSMS;
            }
            else
            {
                return BadRequest(new { Message = "Nieprawidłowy typ zmiany." });
            }

            if (existingUser != null)
            {
                return BadRequest(new { Message = $"Użytkownik z nowym {(request.ChangeType == ChangeType.Email ? "adresem email" : "numerem telefonu")} już istnieje." });
            }

            var verificationCode = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = verificationCode;
            user.UpdatedAt = DateTime.Now;

            storage[user.UserId] = request.NewValue;

            await _context.SaveChangesAsync();

            //sendVerification(request.NewValue, verificationCode);

            return Ok(new { VerificationCode = verificationCode });
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik z podanym identyfikatorem nie istnieje." });
            }

            if (!AuthenticationHelper.VerifyPassword(request.OldPassword, user.PasswordHash))
            {
                return BadRequest(new { Message = "Nieprawidłowe stare hasło." });
            }

            if (request.NewPassword != request.ConfirmNewPassword)
            {
                return BadRequest(new { Message = "Hasła nie są zgodne." });
            }

            user.PasswordHash = AuthenticationHelper.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Hasło zostało zmienione." });
        }


        [HttpPost("verifyChange")]
        public async Task<IActionResult> VerifyChange(VerifyRequestModel request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik z podanym identyfikatorem nie istnieje lub nie został zweryfikowany." });
            }

            if (user.VerificationCode != request.VerificationCode)
            {
                return BadRequest(new { Message = "Nieprawidłowy kod weryfikacyjny." });
            }

            if (request.ChangeType == ChangeType.Email)
            {
                if (!_emailVerificationStorage.TryGetValue(request.UserId, out var newEmail))
                {
                    return BadRequest(new { Message = "Kod weryfikacyjny wygasł lub jest nieprawidłowy." });
                }

                user.Email = newEmail;
                _emailVerificationStorage.TryRemove(request.UserId, out _);
            }
            else if (request.ChangeType == ChangeType.Phone)
            {
                if (!_phoneVerificationStorage.TryGetValue(request.UserId, out var newPhone))
                {
                    return BadRequest(new { Message = "Kod weryfikacyjny wygasł lub jest nieprawidłowy." });
                }

                user.PhoneNumber = newPhone;
                _phoneVerificationStorage.TryRemove(request.UserId, out _);
            }

            user.VerificationCode = null;
            user.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Zmiana została zweryfikowana." });
        }
    }
}