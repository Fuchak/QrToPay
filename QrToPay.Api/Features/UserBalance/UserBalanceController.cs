using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Features.UserBalance.TopUp;
using QrToPay.Api.Features.UserBalance.CheckBalance;
using MediatR;

namespace QrToPay.Api.Features.UserBalance
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserBalanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserBalanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId:int}/balance")]
        public async Task<IActionResult> GetUserBalance([FromRoute] int userId)
        {
            GetUserBalanceRequestModel request = new() { UserId = userId };
            var result = await _mediator.Send(request);

            return !result.IsSuccess 
                ? NotFound(new { Message = result.Error }) 
                : Ok(result.Value);
        }

        [HttpPost("topup")]
        public async Task<IActionResult> TopUpAccount([FromBody] TopUpRequestModel request)
        {
            var result = await _mediator.Send(request);

            return !result.IsSuccess 
                ? StatusCode(500, new { Message = result.Error }) 
                : Ok(new { accountBalance = result.Value });
        }
    }
}