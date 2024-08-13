using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Features.UserBalance.TopUp;
using QrToPay.Api.Models;
using QrToPay.Api.Features.UserBalance.CheckBalance;

namespace QrToPay.Api.Features.UserBalance
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserBalanceController : ControllerBase
    {
        private readonly QrToPayDbContext _context;

        public UserBalanceController(QrToPayDbContext context)
        {
            _context = context;
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