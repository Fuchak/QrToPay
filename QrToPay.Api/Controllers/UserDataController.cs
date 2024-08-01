using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Features.Auth.Login;
using QrToPay.Api.Models;
using QrToPay.Api.Requests;
using QrToPay.Api.Responses;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDataController : ControllerBase
    {
        private readonly QrToPayDbContext _context;

        public UserDataController(QrToPayDbContext context)
        {
           _context = context;
        }

        //Narazie nie używamy tego może będziemy używać po zmienieniu telefonu czy tam emaila w ustawieniach jak nie to out robi
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserDetails([FromRoute] int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            LoginDto loginDto = new()
            {
                UserId = user.UserId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AccountBalance = user.AccountBalance
            };

            return Ok(loginDto);
        }

        [HttpGet("{userId:int}/balance")]
        public async Task<IActionResult> GetUserBalance([FromRoute] int userId)
        {
            var balance = await _context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new UserBalanceDto { AccountBalance = u.AccountBalance })
                .FirstOrDefaultAsync();

            if (balance == null)
            {
                return NotFound();
            }

            return Ok(balance);
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUpAccount([FromBody] TopUpRequestModel request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound("Użytkownik nieodnaleziony");
            }

            user.AccountBalance = (user.AccountBalance ?? 0) + request.Amount;
            user.UpdatedAt = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var updatedBalance = user.AccountBalance;

            return Ok(new { accountBalance = updatedBalance });
        }
    }
}