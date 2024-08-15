using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Features.SkiSlopes.Prices;
using QrToPay.Api.Features.SkiSlopes.Slopes;
using MediatR;

//TODO te miasta zmienić spowrotem do skislopes i funfairs i wywalić je z entities 
namespace QrToPay.Api.Features.SkiSlopes
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkiSlopesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SkiSlopesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/skislopes
        [HttpGet("slopes")]
        public async Task<IActionResult> GetSkiSlopes([FromQuery] GetSkiSlopesRequestModel request)
        {
            var result = await _mediator.Send(request);

            return !result.IsSuccess 
                ? StatusCode(500, new { Message = result.Error }) 
                : Ok(result.Value);
        }

        // GET: api/skislopes/prices
        [HttpGet("prices")]
        public async Task<IActionResult> GetSkiSlopePrices([FromQuery] GetSkiSlopePricesRequestModel request)
        {
            var result = await _mediator.Send(request);

            return !result.IsSuccess 
                ? StatusCode(500, new { Message = result.Error }) 
                : Ok(result.Value);
        }
    }
}