using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Common.Results;
using Microsoft.AspNetCore.Authorization;

namespace QrToPay.Api.Features.Cities;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary> Get cities of opened type </summary>
    /// <response code="404">Not Found </response>
    /// <response code="401">Unauthorized </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpGet]
    public async Task<IActionResult> GetCities([FromQuery] GetCitiesRequestModel request)
    {
        Result<IEnumerable<CitiesDto>> result = await _mediator.Send(request);

        return result.ToActionResult();
    }
}