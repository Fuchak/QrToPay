using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.DTOs;
using QrToPay.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace QrToPay.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkiSlopesController(QrToPayDbContext context) : ControllerBase
    {

        // GET: api/skislopes
        [HttpGet("slopes")]
        public async Task<ActionResult<IEnumerable<SkiSlopeDto>>> GetSkiSlopes(int cityId)
        {
            var skiSlopes = await context.SkiSlopes
                .Include(s => s.City)
                .Where(s => !s.IsDeleted && !s.City.IsDeleted && s.CityId == cityId)
                .Select(s => new SkiSlopeDto
                {
                    SkiResortId = s.SkiResortId,
                    ResortName = s.ResortName,
                    CityName = s.City.CityName
                })
                .ToListAsync();

            return Ok(skiSlopes);
        }

        // GET: api/skislopes/prices
        [HttpGet("prices")]
        public async Task<ActionResult<IEnumerable<SkiSlopePriceDto>>> GetSkiSlopePrices(int skiResortId)
        {
            var prices = await context.SkiSlopePrices
                .Where(p => p.SkiResortId == skiResortId && !p.IsDeleted)
                .Select(p => new { p.Tokens, p.Price, p.SkiSlopePriceId })
                .Distinct()
                .Select(p => new SkiSlopePriceDto
                {
                    SkiSlopePriceID = p.SkiSlopePriceId,
                    Tokens = p.Tokens,
                    Price = p.Price
                })
                .ToListAsync();

            return Ok(prices);
        }

    }
}
