using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.DTOs;
using QrToPay.Api.Models;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(QrToPayDbContext context) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto loginDto)
        {
            var userQuery = context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(loginDto.Email))
            {
                userQuery = userQuery.Where(u => u.Email == loginDto.Email);
            }
            else if (!string.IsNullOrEmpty(loginDto.PhoneNumber))
            {
                userQuery = userQuery.Where(u => u.PhoneNumber == loginDto.PhoneNumber);
            }
            else
            {
                return BadRequest(new { Message = "Email lub numer telefonu jest wymagany." });
            }

            var user = await userQuery.FirstOrDefaultAsync();

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.PasswordHash, user.PasswordHash))
            {
                return Unauthorized(new { Message = "Nieprawidłowy email, numer telefonu lub hasło." });
            }

            if (!user.IsVerified)
            {
                return Unauthorized(new { Message = "Konto nie zostało aktywowane." });
            }

            if (user.IsDeleted)
            {
                return Unauthorized(new { Message = "Konto zostało zablokowane." });
            }

            var userDto = new UserDto
            {
                UserID = user.UserId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AccountBalance = user.AccountBalance
            };

            return Ok(userDto);
        }
    }
}
