using MongoDB.Driver;
using AirlineReservation.Models.Data;

namespace AirlineReservation.Services.Database
{
    public interface IDatabaseContext
    {
        IMongoCollection<AirlineModel> Airlines { get; }
    }
}
