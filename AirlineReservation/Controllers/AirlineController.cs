using AirlineReservation.Services.Airline;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservation.Controllers
{
    [Route("api")]
    [ApiController]
    public class AirlineController : ControllerBase
    {
        private readonly IAirlineService _airlineService;

        public AirlineController(IAirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        [HttpGet("getAllAirline")]
        public async Task<IActionResult> GetAllAirlines()
        {
            var airlines = await _airlineService.getAllAirlines();
            return Ok(airlines);
        }
    }
}
