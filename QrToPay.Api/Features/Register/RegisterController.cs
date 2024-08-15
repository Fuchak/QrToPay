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

        return !result.IsSuccess 
            ? StatusCode(500, new { Message = result.Error }) 
            : Ok(result.Value);
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyCreateUserRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? StatusCode(500, new { Message = result.Error }) 
            : Ok(new { Message = result.Value });
    }
}
