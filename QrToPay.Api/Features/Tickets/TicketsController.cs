using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Tickets.Active;
using QrToPay.Api.Features.Tickets.History;
using QrToPay.Api.Features.Tickets.Purchase;
using QrToPay.Api.Features.Tickets.Activate;
using QrToPay.Api.Features.Tickets.Deactivate;

namespace QrToPay.Api.Features.Tickets;

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
    public async Task<IActionResult> GetActiveTickets([FromQuery] GetActiveTicketsRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? StatusCode(500, new { Message = result.Error }) 
            : Ok(result.Value);
    }

    [HttpPost("generateAndUpdate")]
    public async Task<IActionResult> GenerateAndUpdateTicket([FromBody] PurchaseTicketRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? StatusCode(500, new { Message = result.Error }) 
            : Ok(new { qrCode = result.Value });
    }

    //[GitHub issue `#54212`](https://github.com/dotnet/aspnetcore/issues/54212).
    #pragma warning disable ASP0018
    [HttpGet("getHistory/{userId}")]
    public async Task<IActionResult> GetTicketHistory([FromRoute] GetTicketHistoryRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess 
            ? StatusCode(500, new { Message = result.Error }) 
            : Ok(result.Value);
    }
    #pragma warning restore ASP0018

    [HttpPost("activate")]
    public async Task<IActionResult> ActivateQrCode([FromBody] ActivateQrCodeRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess
            ? StatusCode(500, new { Message = result.Error })
            : Ok(result.Value);
    }

    [HttpPost("deactivate")]
    public async Task<IActionResult> DeactivateQrCode([FromBody] DeactivateQrCodeRequestModel request)
    {
        var result = await _mediator.Send(request);

        return !result.IsSuccess
            ? StatusCode(500, new { Message = result.Error })
            : Ok();
    }
}