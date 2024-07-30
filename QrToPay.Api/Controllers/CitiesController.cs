using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.Models;
using QrToPay.Api.Responses;

namespace QrToPay.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly QrToPayDbContext _context;

        public CitiesController(QrToPayDbContext context)
        {
            _context = context;
        }

        [HttpGet("{attractionType}")]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities(string attractionType)
        {
            List<CityDto> cities;

            if (attractionType == "skislope")
            {
                cities = await _context.SkiSlopes
                    .Where(s => !s.IsDeleted)
                    .Select(s => new CityDto
                    {
                        CityName = s.CityName,
                        EntityId = s.EntityId
                    })
                    .ToListAsync();
            }
            else if (attractionType == "funfair")
            {
                cities = await _context.FunFairs
                    .Where(f => !f.IsDeleted)
                    .Select(f => new CityDto
                    {
                        CityName = f.CityName,
                        EntityId = f.EntityId
                    })
                    .ToListAsync();
            }
            else
            {
                return BadRequest("Invalid attraction type.");
            }

            return Ok(cities);
        }
    }
}
