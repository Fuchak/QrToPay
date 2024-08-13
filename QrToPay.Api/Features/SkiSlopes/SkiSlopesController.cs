using Microsoft.AspNetCore.Mvc;
using QrToPay.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using QrToPay.Api.Common.Dtos;
using QrToPay.Api.Features.SkiSlopes.Prices;
using QrToPay.Api.Features.SkiSlopes.Slopes;

//TODO te miasta zmienić spowrotem do skislopes i funfairs i wywalić je z entities 
namespace QrToPay.Api.Features.SkiSlopes
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkiSlopesController : ControllerBase
    {
        private readonly QrToPayDbContext _context;

        public SkiSlopesController(QrToPayDbContext context)
        {
            _context = context;
        }

        // GET: api/skislope/cities
        [HttpGet("cities")]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
        {
            var cities = await _context.SkiSlopes
                .Where(s => !s.IsDeleted)
                .Select(s => new CityDto
                {
                    CityName = _context.Entities
                        .Where(e => e.EntityId == s.EntityId && !e.IsDeleted)
                        .Select(e => e.CityName)
                        .FirstOrDefault()!, // Używamy CityName z tabeli Entities i informujemy kompilator, że nie będzie null
                    EntityId = s.EntityId
                })
                .ToListAsync();

            if (cities.Count is 0)
            {
                Debug.WriteLine("No cities found for SkiSlopes.");
            }

            return Ok(cities);
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
                    ResortName = _context.Entities
                        .Where(e => e.EntityId == s.EntityId && !e.IsDeleted)
                        .Select(e => e.EntityName)
                        .FirstOrDefault()!, // Używamy EntityName z tabeli Entities i informujemy kompilator, że nie będzie null
                    CityName = _context.Entities
                        .Where(e => e.EntityId == s.EntityId && !e.IsDeleted)
                        .Select(e => e.CityName)
                        .FirstOrDefault()!, // Używamy CityName z tabeli Entities i informujemy kompilator, że nie będzie null
                })
                .ToListAsync();

            if (skiSlopes.Count is 0)
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