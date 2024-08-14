using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Auth.Login;
using QrToPay.Api.Common.Results;
using QrToPay.Api.Features.Auth.ResetPassword;

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
        if (!result.IsSuccess)
        {
            return Unauthorized(new { Message = result.Error });
        }
        return Ok(result.Value);
    }

    [HttpPost("contact")]
    public async Task<IActionResult> UserExist([FromBody] UserExistRequestModel request)
    {
        var result = await _mediator.Send(request);
        if (!result.IsSuccess)
        {
            return StatusCode(500, new { Message = result.Error });
        }
        return Ok(new { VerificationCode = result.Value });
    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestModel request)
    {
        var result = await _mediator.Send(request);
        if (!result.IsSuccess)
        {
            return StatusCode(500, new { Message = result.Error });
        }
        return Ok(new { Message = "Hasło zostało zaktualizowane." });
    }

}