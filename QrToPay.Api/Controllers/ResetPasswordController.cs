using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Data;
using QrToPay.Api.DTOs;
using QrToPay.Api.Helpers;
using QrToPay.Api.Models;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResetPasswordController(DatabaseContext context) : ControllerBase
    {

        [HttpPost("email")]
        public async Task<IActionResult> CheckIfEmailExist(UserExistRequest request)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik z podanym e-mailem nie istnieje." });
            }

            if (!user.IsVerified)
            {
                return BadRequest(new { Message = "Konto użytkownika nie zostało potwierdzone." });
            }

            var verificationCode = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = verificationCode;
            await context.SaveChangesAsync();

            return Ok(new { VerificationCode = verificationCode });
        }

        [HttpPost("phone")]
        public async Task<IActionResult> CheckIfPhoneExist(UserExistRequest request)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik z podanym numerem telefonu nie istnieje." });
            }

            if (!user.IsVerified)
            {
                return BadRequest(new { Message = "Konto użytkownika nie zostało potwierdzone." });
            }

            var verificationCode = AuthenticationHelper.GenerateVerificationCode();
            user.VerificationCode = verificationCode;
            await context.SaveChangesAsync();

            return Ok(new { VerificationCode = verificationCode });
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest(new { Message = "Nowe hasło jest wymagane." });
            }

            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone);

            if (user == null)
            {
                return NotFound(new { Message = "Użytkownik z podanym e-mailem lub numerem telefonu nie istnieje." });
            }

            if (user.VerificationCode != request.VerificationCode)
            {
                return BadRequest(new { Message = "Nieprawidłowy kod weryfikacyjny." });
            }

            // Update password
            user.PasswordHash = AuthenticationHelper.HashPassword(request.NewPassword);
            user.VerificationCode = null;
            await context.SaveChangesAsync();

            return Ok(new { Message = "Hasło zostało zaktualizowane." });
        }
    }
}
