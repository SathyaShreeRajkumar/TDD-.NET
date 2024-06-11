using AirlineReservation.Models.Api;
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

        [HttpPost]
        public async Task<IActionResult> CreateAirline(AirlineDto airlineDto)
        {
            var createAirline = await _airlineService.CreateAirline(airlineDto);
            return createAirline == null ? NotFound() : CreatedAtAction(nameof(CreateAirline),
            new { id = createAirline.AirlineId }, createAirline);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAirline(string airlineId)
        {
            var count = await _airlineService.DeleteAirline(airlineId);
            return count == 0 ? NotFound() : Ok(count);
        }
    }
}
