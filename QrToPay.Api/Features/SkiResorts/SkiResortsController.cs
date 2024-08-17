using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.SkiResorts.Prices;
using QrToPay.Api.Features.SkiResorts.Resorts;

namespace QrToPay.Api.Features.SkiResorts;

[ApiController]
[Route("api/[controller]")]
public class SkiResortsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SkiResortsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/skiresorts
    [HttpGet("resorts")]
    public async Task<IActionResult> GetSkiSlopes([FromQuery] GetSkiResortsRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess
            ? StatusCode(500, new { Message = result.Error })
            : Ok(result.Value);
    }

    // GET: api/skiresorts/prices
    [HttpGet("prices")]
    public async Task<IActionResult> GetSkiSlopePrices([FromQuery] GetSkiResortPricesRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess
            ? StatusCode(500, new { Message = result.Error })
            : Ok(result.Value);
    }
}