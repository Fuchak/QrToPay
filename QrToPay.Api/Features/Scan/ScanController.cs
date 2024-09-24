using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Scan.Purchase;
using QrToPay.Api.Features.Scan.ScanedInfo;
using QrToPay.Api.Common.Results;
using Microsoft.AspNetCore.Authorization;

namespace QrToPay.Api.Features.Scan;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ScanController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScanController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary> Purchase ticket by scanning qr code </summary>
    /// <response code="404">Not Found </response>
    /// <response code="401">Unauthorized </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseTicket([FromBody] ScanPurchaseRequestModel request)
    {
        Result<SuccesMessageDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }

    /// <summary> Purchase ticket by scanning qr code </summary>
    /// <response code="404">Not Found </response>
    /// <response code="401">Unauthorized </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
#pragma warning disable ASP0018
    //[GitHub issue `#54212`](https://github.com/dotnet/aspnetcore/issues/54212).
    [HttpGet("{qrCode}")]
    public async Task<IActionResult> GetAttractionByQrCode([FromRoute] ScanedInfoRequestModel request)
    {
        Result<ScanedInfoDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }
    #pragma warning restore ASP0018
}