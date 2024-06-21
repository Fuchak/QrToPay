using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QrToPay.Api.DTOs;
using QrToPay.Api.Models;


namespace QrToPay.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CitiesController(QrToPayDbContext context) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
        {
            var cities = await context.Cities
                .Where(c => !c.IsDeleted)
                .Select(c => new CityDto
                {
                    CityId = c.CityId,
                    CityName = c.CityName
                })
                .ToListAsync();

            return Ok(cities);
        }
    }
}
