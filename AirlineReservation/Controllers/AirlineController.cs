using AirlineReservation.Services.Airline;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservation.Controllers
{
    [Route("api/airlines")]
    [ApiController]
    public class AirlineController : ControllerBase
    {
        private readonly IAirlineService _airlineService;

        public AirlineController(IAirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAirlines()
        {
            var airlines = await _airlineService.GetAllAirlines();
            return Ok(airlines);
        }

        [HttpGet("/{airlineId}")]
        public async Task<IActionResult> GetAirlineById(string airlineId)
        {
            var airline = await _airlineService.GetAirlineById(airlineId);
            return airline == null ? NotFound() : Ok(airline);
        }
    }
}
