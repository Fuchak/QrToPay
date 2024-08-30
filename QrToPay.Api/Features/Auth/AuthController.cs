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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
    {
        Result<LoginDto> result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? Unauthorized(new { Message = result.Error }) 
            : Ok(result.Value);
    }

    [HttpPost("checkAccount")]
    public async Task<IActionResult> UserExist([FromBody] CheckAccountRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? StatusCode(500, new { Message = result.Error }) 
            : Ok(new { VerificationCode = result.Value });
    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? StatusCode(500, new { Message = result.Error }) 
            : Ok(new { Message = result.Value });
    }
}