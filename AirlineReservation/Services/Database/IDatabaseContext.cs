using MongoDB.Driver;
using Airline_Reservation.Models.Data;

namespace AirlineReservation.Services.Database
{
    public interface IDatabaseContext
    {
        IMongoCollection<AirlineModel> Airlines { get; }
    }
}
