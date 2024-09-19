using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.FunFairs.Prices;
using QrToPay.Api.Features.FunFairs.FunFairs;
using QrToPay.Api.Common.Results;

namespace QrToPay.Api.Features.FunFairs;

[ApiController]
[Route("api/[controller]")]
public class FunFairsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FunFairsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary> Gets funfairs from selected city </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpGet("city")]
    public async Task<IActionResult> GetFunFairs([FromQuery] GetFunFairsRequestModel request)
    {
        Result<IEnumerable<FunFairsDto>> result = await _mediator.Send(request);

        return result.ToActionResult();
    }

    /// <summary> Gets funfair prices of all slopes </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpGet("prices")]
    public async Task<IActionResult> GetFunFairsPrices([FromQuery] GetFunFairPricesRequestModel request)
    {
        Result<IEnumerable<FunFairPriceDto>> result = await _mediator.Send(request);

        return result.ToActionResult();
    }
}