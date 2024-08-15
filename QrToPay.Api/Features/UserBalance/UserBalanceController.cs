using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Features.UserBalance.TopUp;
using QrToPay.Api.Features.UserBalance.CheckBalance;
using MediatR;

namespace QrToPay.Api.Features.UserBalance;

[ApiController]
[Route("api/[controller]")]
public class UserBalanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserBalanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //[GitHub issue `#54212`](https://github.com/dotnet/aspnetcore/issues/54212).
    #pragma warning disable ASP0018
    [HttpGet("{userId:int}/balance")]
    public async Task<IActionResult> GetUserBalance([FromRoute] GetUserBalanceRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? NotFound(new { Message = result.Error }) 
            : Ok(result.Value);
    }
    #pragma warning restore ASP0018

    [HttpPost("topup")]
    public async Task<IActionResult> TopUpAccount([FromBody] TopUpRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? StatusCode(500, new { Message = result.Error }) 
            : Ok(new { accountBalance = result.Value });
    }
}