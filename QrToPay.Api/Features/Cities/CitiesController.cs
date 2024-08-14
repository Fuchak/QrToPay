using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Common.Enums;

namespace QrToPay.Api.Features.Cities;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/cities
    [HttpGet]
    public async Task<IActionResult> GetCities([FromQuery] EntityCategory entityType)
    {
        var request = new GetCitiesRequestModel { EntityType = entityType };
        var result = await _mediator.Send(request);

        if (!result.IsSuccess)
        {
            return StatusCode(500, new { Message = result.Error });
        }
        return Ok(result.Value);
    }
}