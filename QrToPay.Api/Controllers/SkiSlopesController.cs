using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using QrToPay.Api.Responses;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkiSlopesController: ControllerBase
    {
        private readonly QrToPayDbContext _context;

        public SkiSlopesController(QrToPayDbContext context)
        {
            _context = context;
        }
        // GET: api/skislopes
        [HttpGet("slopes")]
        public async Task<ActionResult<IEnumerable<SkiSlopeDto>>> GetSkiSlopes(Guid entityId)
        {
            var skiSlopes = await _context.SkiSlopes
                .Where(s => !s.IsDeleted && s.EntityId == entityId)
                .Select(s => new SkiSlopeDto
                {
                    SkiResortId = s.SkiResortId,
                    ResortName = s.ResortName,
                    CityName = s.CityName
                })
                .ToListAsync();

            if (!skiSlopes.Any())
            {
                Debug.WriteLine($"No ski slopes found for entityId: {entityId}");
            }

            return Ok(skiSlopes);
        }

        // GET: api/skislopes/prices
        [HttpGet("prices")]
        public async Task<ActionResult<IEnumerable<SkiSlopePriceDto>>> GetSkiSlopePrices([FromQuery] int skiResortId)
        {
            var prices = await _context.SkiSlopePrices
                .Where(p => p.SkiResortId == skiResortId && !p.IsDeleted)
                .Select(p => new { p.Tokens, p.Price, p.SkiSlopePriceId })
                .Distinct()
                .Select(p => new SkiSlopePriceDto
                {
                    SkiSlopePriceId = p.SkiSlopePriceId,
                    Tokens = p.Tokens,
                    Price = p.Price
                })
                .ToListAsync();

            return Ok(prices);
        }

    }
}