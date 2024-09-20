using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Features.UserBalance.TopUp;
using QrToPay.Api.Features.UserBalance.CheckBalance;
using MediatR;
using QrToPay.Api.Common.Results;
using Microsoft.AspNetCore.Authorization;

namespace QrToPay.Api.Features.UserBalance;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserBalanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserBalanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary> Get account balance. Optional UserID. If not passed will be taken out from JWT token</summary>
    /// <response code="404">Not Found </response>
    /// <response code="401">Unauthorized </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpGet("balance")]
    public async Task<IActionResult> GetUserBalance([FromQuery] GetUserBalanceRequestModel request)
    {
        Result<UserBalanceDto> result = await _mediator.Send(request);
        return result.ToActionResult();
    }

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