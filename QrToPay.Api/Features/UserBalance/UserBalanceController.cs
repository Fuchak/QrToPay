using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Features.UserBalance.TopUp;
using QrToPay.Api.Features.UserBalance.CheckBalance;
using MediatR;
using QrToPay.Api.Common.Results;

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

    /// <summary> Get account balance </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    #pragma warning disable ASP0018
    //[GitHub issue `#54212`](https://github.com/dotnet/aspnetcore/issues/54212).
    [HttpGet("{userId:int}/balance")]
    public async Task<IActionResult> GetUserBalance([FromRoute] GetUserBalanceRequestModel request)
    {
        Result<UserBalanceDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }
    #pragma warning restore ASP0018

    /// <summary> Topup account with money </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost("topup")]
    public async Task<IActionResult> TopUpAccount([FromBody] TopUpRequestModel request)
    {
        Result<decimal> result = await _mediator.Send(request);

        return result.ToActionResult();
    }
}