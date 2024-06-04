using Airline_Reservation.Models.Api;
using Airline_Reservation.Models.Data;

namespace Airline_Reservation.Services.Airline
{
    public interface IAirlineService
    {

        Task<List<AirlineModel>> getAllAirlines();
    }
}
