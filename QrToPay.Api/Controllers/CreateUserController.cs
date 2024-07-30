using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Helpers;
using QrToPay.Api.Requests;
using QrToPay.Api.Responses;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly QrToPayDbContext _context;

        public RegisterController(QrToPayDbContext context)
        {
            _context = context;
        }

        [HttpPost("email")]
        public async Task<IActionResult> RegisterWithEmail(CreateUserRequestModel request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingUser != null)
            {
                if (existingUser.IsVerified)
                {
                    return BadRequest(new { Message = "Użytkownik z podanym e-mailem już istnieje." });
                }

                // Aktualizuj dane istniejącego użytkownika
                existingUser.PasswordHash = AuthenticationHelper.HashPassword(request.PasswordHash);
                existingUser.VerificationCode = AuthenticationHelper.GenerateVerificationCode();
                existingUser.IsVerified = false;
                existingUser.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            else
            {
                var newUser = new User
                {
                    Email = request.Email,
                    PasswordHash = AuthenticationHelper.HashPassword(request.PasswordHash),
                    VerificationCode = AuthenticationHelper.GenerateVerificationCode(),
                    IsVerified = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
            }

            // Znajdź użytkownika, aby zwrócić kod weryfikacyjny
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return StatusCode(500, new { Message = "Błąd wewnętrzny serwera." });
            }

            // Zwróć kod weryfikacyjny
            return Ok(new CreateUserDto { VerificationCode = user.VerificationCode });
        }

        [HttpPost("phone")]
        public async Task<IActionResult> RegisterWithPhone(CreateUserRequestModel request)
        {

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (existingUser != null)
            {
                if (existingUser.IsVerified)
                {
                    return BadRequest(new { Message = "Użytkownik z podanym numerem telefonu już istnieje." });
                }

                // Aktualizuj dane istniejącego użytkownika
                existingUser.PasswordHash = AuthenticationHelper.HashPassword(request.PasswordHash);
                existingUser.VerificationCode = AuthenticationHelper.GenerateVerificationCode();
                existingUser.IsVerified = false;
                existingUser.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
            }
            else
            {
                var newUser = new User
                {
                    PhoneNumber = request.PhoneNumber,
                    PasswordHash = AuthenticationHelper.HashPassword(request.PasswordHash),
                    VerificationCode = AuthenticationHelper.GenerateVerificationCode(),
                    IsVerified = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
            }

            // Znajdź użytkownika, aby zwrócić kod weryfikacyjny
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (user == null)
            {
                return StatusCode(500, new { Message = "Błąd wewnętrzny serwera." });
            }

            // Zwróć kod weryfikacyjny
            return Ok(new CreateUserDto { VerificationCode = user.VerificationCode });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser(VerifyCreateUserRequestModel request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => (u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone) && u.VerificationCode == request.VerificationCode);

            if (user == null)
            {
                return BadRequest(new { Message = "Nieprawidłowy kod weryfikacyjny." });
            }

            user.IsVerified = true;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Użytkownik został zweryfikowany." });
        }
    }
}