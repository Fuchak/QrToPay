using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.SkiResorts.Prices;
using QrToPay.Api.Features.SkiResorts.Resorts;
using QrToPay.Api.Common.Results;

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

    /// <summary> Gets skiresort from selected resort </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpGet("resorts")]
    public async Task<IActionResult> GetSkiResorts([FromQuery] GetSkiResortsRequestModel request)
    {
        Result<IEnumerable<SkiResortsDto>> result = await _mediator.Send(request);

        return result.ToActionResult();
    }

    /// <summary> Gets skiresort prices of all slopes </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpGet("prices")]
    public async Task<IActionResult> GetSkiResortsPrices([FromQuery] GetSkiResortPricesRequestModel request)
    {
        Result<IEnumerable<SkiResortPriceDto>> result = await _mediator.Send(request);

        return result.ToActionResult();
    }
}