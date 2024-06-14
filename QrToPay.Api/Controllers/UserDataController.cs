using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.DTOs;
using QrToPay.Api.Data;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserDataController(DatabaseContext context) : ControllerBase
    {

        //Narazie nie używamy tego może będziemy używać po zmienieniu telefonu czy tam emaila w ustawieniach jak nie to out robi
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetUserDetails(int userID)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.UserID == userID);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                UserID = user.UserID,
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
                .Where(u => u.UserID == userID)
                .Select(u => new UserBalanceRequest { AccountBalance = u.AccountBalance })
                .FirstOrDefaultAsync();

            if (balance == null)
            {
                return NotFound();
            }

            return Ok(balance);
        }
    }
}
