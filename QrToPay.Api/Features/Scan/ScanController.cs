using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Scan.Purchase;
using QrToPay.Api.Features.Scan.ScanedInfo;

namespace QrToPay.Api.Features.Scan;

[ApiController]
[Route("api/[controller]")]
public class ScanController : ControllerBase
{
    private readonly IMediator _mediator;

    public ScanController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //[GitHub issue `#54212`](https://github.com/dotnet/aspnetcore/issues/54212).
    #pragma warning disable ASP0018
    [HttpGet("{qrCode}")]
    public async Task<IActionResult> GetAttractionByQrCode([FromRoute] ScanedInfoRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? BadRequest( new { Message = result.Error }) 
            : Ok(result.Value);
    }
    #pragma warning restore ASP0018

    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseTicket([FromBody] ScanPurchaseRequestModel request)
    {
        var result = await _mediator.Send(request);
        return !result.IsSuccess 
            ? BadRequest( new { Message = result.Error }) 
            : Ok(new { Message = result.Value });
    }
}