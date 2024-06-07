using AirlineReservation.Models.Data;

namespace AirlineReservation.Services.Airline
{
    public interface IAirlineService
    {
        Task<List<AirlineModel>> GetAllAirlines();

        Task<AirlineModel> GetAirlineById(string id);
    }
}
