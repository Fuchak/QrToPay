using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.DTOs;
using QrToPay.Api.Data;
using QrToPay.Api.Models;
using QrToPay.Api.Helpers;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController(DatabaseContext context) : ControllerBase
    {
        [HttpPost("email")]
        public async Task<IActionResult> RegisterWithEmail(CreateUserRequest request)
        {
            var existingUser = await context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                if (existingUser.IsVerified)
                {
                    return BadRequest(new { Message = "Użytkownik z podanym e-mailem już istnieje." });
                }

                // Aktualizuj dane istniejącego użytkownika
                existingUser.PasswordHash = AuthenticationHelper.HashPassword(request.Password);
                existingUser.VerificationCode = AuthenticationHelper.GenerateVerificationCode();
                existingUser.IsVerified = false;

                await context.SaveChangesAsync();
            }
            else
            {
                var newUser = new User
                {
                    Email = request.Email,
                    PasswordHash = AuthenticationHelper.HashPassword(request.Password),
                    VerificationCode = AuthenticationHelper.GenerateVerificationCode(),
                    IsVerified = false
                };

                context.Users.Add(newUser);
                await context.SaveChangesAsync();
            }

            // Znajdź użytkownika, aby zwrócić kod weryfikacyjny
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return StatusCode(500, new { Message = "Błąd wewnętrzny serwera." });
            }

            // Zwróć kod weryfikacyjny
            return Ok(new CreateUserResponse { VerificationCode = user.VerificationCode });
        }

        [HttpPost("phone")]
        public async Task<IActionResult> RegisterWithPhone(CreateUserRequest request)
        {

            var existingUser = await context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (existingUser != null)
            {
                if (existingUser.IsVerified)
                {
                    return BadRequest(new { Message = "Użytkownik z podanym numerem telefonu już istnieje." });
                }

                // Aktualizuj dane istniejącego użytkownika
                existingUser.PasswordHash = AuthenticationHelper.HashPassword(request.Password);
                existingUser.VerificationCode = AuthenticationHelper.GenerateVerificationCode();
                existingUser.IsVerified = false;

                await context.SaveChangesAsync();
            }
            else
            {
                var newUser = new User
                {
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = AuthenticationHelper.HashPassword(request.Password),
                    VerificationCode = AuthenticationHelper.GenerateVerificationCode(),
                    IsVerified = false
                };

                context.Users.Add(newUser);
                await context.SaveChangesAsync();
            }

            // Znajdź użytkownika, aby zwrócić kod weryfikacyjny
            var user = await context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (user == null)
            {
                return StatusCode(500, new { Message = "Błąd wewnętrzny serwera." });
            }

            // Zwróć kod weryfikacyjny
            return Ok(new CreateUserResponse { VerificationCode = user.VerificationCode });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser(VerifyRequest request)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => (u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone) && u.VerificationCode == request.VerificationCode);

            if (user == null)
            {
                return BadRequest(new { Message = "Nieprawidłowy kod weryfikacyjny." });
            }

            user.IsVerified = true;
            await context.SaveChangesAsync();

            return Ok(new { Message = "Użytkownik został zweryfikowany." });
        }
    }
}