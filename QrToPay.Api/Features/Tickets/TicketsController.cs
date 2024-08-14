using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Tickets.Active;
using QrToPay.Api.Features.Tickets.History;
using QrToPay.Api.Features.Tickets.Purchase;

namespace QrToPay.Api.Features.Tickets
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveTickets([FromQuery] int userId)
        {
            GetActiveTicketsRequestModel request = new() { UserId = userId };
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return StatusCode(500, new { Message = result.Error });
            }
            return Ok(result.Value);
        }

        [HttpPost("generateAndUpdate")]
        public async Task<IActionResult> GenerateAndUpdateTicket([FromBody] PurchaseTicketRequestModel request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return StatusCode(500, new { Message = result.Error });
            }
            return Ok(new { qrCode = result.Value });
        }

        [HttpGet("getHistory/{userId}")]
        public async Task<IActionResult> GetTicketHistory(int userId)
        {
            GetTicketHistoryRequestModel request = new() { UserId = userId };
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return StatusCode(500, new { Message = result.Error });
            }
            return Ok(result.Value);
        }
    }
}