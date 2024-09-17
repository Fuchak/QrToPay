using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Auth.Login;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Features.Auth.ResetPassword;
using QrToPay.Api.Features.Auth.CheckAccount;

namespace QrToPay.Api.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary> Logins user into app </summary>
    /// <response code="403">Account not verified </response>
    /// <response code="401">Unauthorized </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
    {
        Result<LoginDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }

    /// <summary> Check if user exist for login </summary>
    /// <response code="404">Not Found </response>
    /// <response code="403">Account not verified </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost("checkAccount")]
    public async Task<IActionResult> CheckAccountExist([FromBody] CheckAccountRequestModel request)
    {
        Result<CheckAccountDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }

    /// <summary> Resets Password </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel request)
    {
        Result<SuccesMessageDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }
}