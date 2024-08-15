using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Scan.Purchase;
using QrToPay.Api.Features.Scan.ScanedInfo;

namespace QrToPay.Api.Features.Scan
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScanController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ScanController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{qrCode:string}")]
        public async Task<IActionResult> GetAttractionByQrCode([FromRoute] string qrCode)
        {
            ScanedInfoRequestModel request = new() { QrCode = qrCode };
            var result = await _mediator.Send(request);

            return !result.IsSuccess 
                ? StatusCode(500, new { Message = result.Error }) 
                : Ok(result.Value);
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseTicket([FromBody] ScanPurchaseRequestModel request)
        {
            var result = await _mediator.Send(request);
            return !result.IsSuccess 
                ? StatusCode(500, new { Message = result.Error }) 
                : Ok(new { Message = result.Value });
        }
    }
}