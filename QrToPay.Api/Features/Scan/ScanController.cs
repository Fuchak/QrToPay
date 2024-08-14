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

        [HttpGet("{qrCode}")]
        public async Task<IActionResult> GetAttractionByQrCode(string qrCode)
        {
            ScanedInfoRequestModel request = new() { QrCode = qrCode };
            var result = await _mediator.Send(request);
            if (!result.IsSuccess)
            {
                return StatusCode(500, new { Message = result.Error });
            }
            return Ok(result.Value);
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> PurchaseTicket([FromBody] ScanPurchaseRequestModel request)
        {
            var result = await _mediator.Send(request);
            if (!result.IsSuccess)
            {
                return StatusCode(500, new { Message = result.Error });
            }
            return Ok(new { Message = result.Value });
        }
    }
}