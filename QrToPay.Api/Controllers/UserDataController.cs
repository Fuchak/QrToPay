using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.DTOs;
using QrToPay.Api.Models;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserDataController(QrToPayDbContext context) : ControllerBase
    {

        //Narazie nie używamy tego może będziemy używać po zmienieniu telefonu czy tam emaila w ustawieniach jak nie to out robi
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetUserDetails(int userID)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.UserId == userID);

            if (user == null)
            {
                return NotFound();
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

        [HttpGet("{userID}/balance")]
        public async Task<IActionResult> GetUserBalance(int userID)
        {
            var balance = await context.Users
                .Where(u => u.UserId == userID)
                .Select(u => new UserBalanceRequest { AccountBalance = u.AccountBalance })
                .FirstOrDefaultAsync();

            if (balance == null)
            {
                return NotFound();
            }

            return Ok(balance);
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUpAccount([FromBody] TopUpRequest request)
        {
            var user = await context.Users.FindAsync(request.UserID);
            if (user == null)
            {
                return NotFound("Użytkownik nieodnaleziony");
            }

            user.AccountBalance = (user.AccountBalance ?? 0) + request.Amount;
            user.UpdatedAt = DateTime.Now;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            var updatedBalance = user.AccountBalance;

            return Ok(new { accountBalance = updatedBalance });
        }
    }
}
