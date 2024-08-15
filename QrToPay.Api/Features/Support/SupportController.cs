using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.ComponentModel.DataAnnotations;
using QrToPay.Api.Features.Support.CreateHelpForm;

namespace QrToPay.Api.Features.Support;

[ApiController]
[Route("api/[controller]")]
public class SupportController : ControllerBase
{
    private readonly IMediator _mediator;

    public SupportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateHelpForm([FromBody, Required] CreateHelpFormRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? StatusCode(500, new { Message = result.Error }) 
            : Ok(new { Message = result.Value });
    }
}