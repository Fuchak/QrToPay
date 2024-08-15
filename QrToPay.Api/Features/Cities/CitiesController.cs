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
    public async Task<IActionResult> GetCities([FromQuery] GetCitiesRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? StatusCode(500, new { Message = result.Error }) 
            : Ok(result.Value);
    }
}