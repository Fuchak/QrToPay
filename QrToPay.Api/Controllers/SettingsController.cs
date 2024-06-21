using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.DTOs;
using QrToPay.Api.Helpers;
using QrToPay.Api.Models;
using System.Collections.Concurrent;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingsController(QrToPayDbContext context) : ControllerBase
    {
        private static ConcurrentDictionary<int, string> emailVerificationStorage = new();
        private static ConcurrentDictionary<int, string> phoneVerificationStorage = new();

        [HttpPost("requestEmailChange")]
        public async Task<IActionResult> RequestEmailChange(ChangeEmailRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik z podanym identyfikatorem nie istnieje lub nie został zweryfikowany." });
            }

            if (!AuthenticationHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return BadRequest(new { Message = "Nieprawidłowe hasło." });
            }

            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == request.NewEmail);

            if (existingUser != null)
            {
                return BadRequest(new { Message = "Użytkownik z nowym adresem e-mail już istnieje." });
            }

            var verificationCode = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = verificationCode;
            user.UpdatedAt = DateTime.Now;

            emailVerificationStorage[user.UserId] = request.NewEmail;

            await context.SaveChangesAsync();

            // W tym miejscu wysyłasz kod weryfikacyjny na nowy adres e-mail
            // SendVerificationCodeEmail(request.NewEmail, verificationCode);

            return Ok(new { VerificationCode = verificationCode });
        }

        [HttpPost("verifyEmailChange")]
        public async Task<IActionResult> VerifyEmailChange(VerifyEmailRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik z podanym identyfikatorem nie istnieje lub nie został zweryfikowany." });
            }

            if (user.VerificationCode != request.VerificationCode)
            {
                return BadRequest(new { Message = "Nieprawidłowy kod weryfikacyjny." });
            }

            if (!emailVerificationStorage.TryGetValue(request.UserId, out var newEmail))
            {
                return BadRequest(new { Message = "Kod weryfikacyjny wygasł lub jest nieprawidłowy." });
            }

            user.Email = newEmail;
            user.VerificationCode = null;
            user.UpdatedAt = DateTime.Now;
            user.IsVerified = true;

            await context.SaveChangesAsync();

            emailVerificationStorage.TryRemove(request.UserId, out _);

            return Ok(new { Message = "Adres e-mail został zaktualizowany." });
        }
        [HttpPost("requestPhoneChange")]
        public async Task<IActionResult> RequestPhoneChange(ChangePhoneRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik z podanym identyfikatorem nie istnieje lub nie został zweryfikowany." });
            }

            if (!AuthenticationHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return BadRequest(new { Message = "Nieprawidłowe hasło." });
            }

            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.NewPhoneNumber);

            if (existingUser != null)
            {
                return BadRequest(new { Message = "Użytkownik z nowym numerem telefonu już istnieje." });
            }

            var verificationCode = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = verificationCode;
            user.UpdatedAt = DateTime.Now;

            phoneVerificationStorage[user.UserId] = request.NewPhoneNumber;

            await context.SaveChangesAsync();

            // W tym miejscu wysyłasz kod weryfikacyjny na nowy numer telefonu
            // SendVerificationCodeSMS(request.NewPhoneNumber, verificationCode);

            return Ok(new { VerificationCode = verificationCode });
        }

        [HttpPost("verifyPhoneChange")]
        public async Task<IActionResult> VerifyPhoneChange(VerifyPhoneRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId && u.IsVerified);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik z podanym identyfikatorem nie istnieje lub nie został zweryfikowany." });
            }

            if (user.VerificationCode != request.VerificationCode)
            {
                return BadRequest(new { Message = "Nieprawidłowy kod weryfikacyjny." });
            }

            if (!phoneVerificationStorage.TryGetValue(request.UserId, out var newPhoneNumber))
            {
                return BadRequest(new { Message = "Kod weryfikacyjny wygasł lub jest nieprawidłowy." });
            }

            user.PhoneNumber = newPhoneNumber;
            user.VerificationCode = null;
            user.UpdatedAt = DateTime.Now;
            user.IsVerified = true;

            await context.SaveChangesAsync();

            phoneVerificationStorage.TryRemove(request.UserId, out _);

            return Ok(new { Message = "Numer telefonu został zaktualizowany." });
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);

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

            await context.SaveChangesAsync();

            return Ok(new { Message = "Hasło zostało zmienione." });
        }
    }
}
