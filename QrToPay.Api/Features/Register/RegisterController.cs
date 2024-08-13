using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Features.Register.CreateUser;
using QrToPay.Api.Features.Register.VerifyCreateUser;

namespace QrToPay.Api.Features.Register;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : ControllerBase
{
    private readonly IMediator _mediator;

    public RegisterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestModel request)
    {
        var result = await _mediator.Send(request);
        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.Error });
        }

        return Ok(result.Value);
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyCreateUserRequestModel request)
    {
        var result = await _mediator.Send(request);
        if (!result.IsSuccess)
        {
            return BadRequest(new { Message = result.Error });
        }

        return Ok(new { Message = result.Value });
    }
}
