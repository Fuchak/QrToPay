using Microsoft.AspNetCore.Mvc;
using MediatR;
using QrToPay.Api.Features.Tickets.Active;
using QrToPay.Api.Features.Tickets.History;
using QrToPay.Api.Features.Tickets.Purchase;
using QrToPay.Api.Features.Tickets.Activate;
using QrToPay.Api.Features.Tickets.Deactivate;
using QrToPay.Api.Common.Results;

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

    /// <summary> Get all active tickets </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpGet("active")]
    public async Task<IActionResult> GetActiveTickets([FromQuery] GetActiveTicketsRequestModel request)
    {
        Result<IEnumerable<ActiveTicketDto>> result = await _mediator.Send(request);

        return result.ToActionResult();
    }

    /// <summary> Purchase ticket by clicking not by scanning </summary>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseTicket([FromBody] PurchaseTicketRequestModel request)
    {
        Result<SuccesMessageDto> result = await _mediator.Send(request);

        return result.ToActionResult(); ;
    }

    /// <summary> Get history of bought tickets </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpGet("history")]
    public async Task<IActionResult> GetTicketHistory([FromQuery] GetTicketHistoryRequestModel request)
    {
        Result<IEnumerable<TicketHistoryDto>> result = await _mediator.Send(request);

        return result.ToActionResult();
    }

    /// <summary> activate ticket </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost("activate")]
    public async Task<IActionResult> ActivateQrCode([FromBody] ActivateQrCodeRequestModel request)
    {
        Result<SuccesMessageDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }

    /// <summary> deactivate ticket </summary>
    /// <response code="404">Not Found </response>
    /// <response code="400">Validation error </response>
    /// <response code="200">Success </response>
    [HttpPost("deactivate")]
    public async Task<IActionResult> DeactivateQrCode([FromBody] DeactivateQrCodeRequestModel request)
    {
        Result<SuccesMessageDto> result = await _mediator.Send(request);

        return result.ToActionResult();
    }
}