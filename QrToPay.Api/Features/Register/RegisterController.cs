using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Common.Results;
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

    /// <summary> Register new user </summary>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequestModel request)
    {
        Result<CreateUserDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }

    /// <summary> Verify Registration </summary>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost("verify")]
    public async Task<IActionResult> Verify([FromBody] VerifyCreateUserRequestModel request)
    {
        Result<SuccesMessageDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }
}