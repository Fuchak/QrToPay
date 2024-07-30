using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Requests;
using QrToPay.Api.Responses;


namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly QrToPayDbContext _context;

        public AuthController(QrToPayDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel request)
        {
            var userQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Email))
            {
                userQuery = userQuery.Where(u => u.Email == request.Email);
            }
            else if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                userQuery = userQuery.Where(u => u.PhoneNumber == request.PhoneNumber);
            }
            else
            {
                return BadRequest(new { Message = "Email lub numer telefonu jest wymagany." });
            }

            var user = await userQuery.FirstOrDefaultAsync();

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.PasswordHash, user.PasswordHash))
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

            LoginDto loginDto = new ()
            {
                UserId = user.UserId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AccountBalance = user.AccountBalance
            };

            return Ok(loginDto);
        }
    }
}
