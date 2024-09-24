using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.ComponentModel.DataAnnotations;
using QrToPay.Api.Features.Support.CreateHelpForm;
using QrToPay.Api.Common.Results;
using Microsoft.AspNetCore.Authorization;

namespace QrToPay.Api.Features.Support;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SupportController : ControllerBase
{
    private readonly IMediator _mediator;

    public SupportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary> Sends helpform to database </summary>
    /// <response code="404">Not Found </response>
    /// <response code="401">Unauthorized </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost]
    public async Task<IActionResult> CreateHelpForm([FromBody, Required] CreateHelpFormRequestModel request)
    {
        Result<SuccesMessageDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }
}