using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Auth.Login;
using FluentValidation;
using FluentValidation.Results;

namespace QrToPay.Api.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<LoginRequestModel> _validator;

    public AuthController(IMediator mediator, IValidator<LoginRequestModel> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(new { Message = validationResult.Errors });
        }

        Result<LoginDto> result = await _mediator.Send(request);
        if (!result.IsSuccess)
        {
            return Unauthorized(new { Message = result.Error });
        }
        return Ok(result.Value);
    }
}