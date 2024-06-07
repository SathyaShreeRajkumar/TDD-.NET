using AirlineReservation.Models.Data;
using AirlineReservation.Services.Database;
using MongoDB.Driver;

namespace AirlineReservation.Services.Airline
{
    public class AirlineService : IAirlineService
    {
        private IDatabaseContext _databaseContext;

        public AirlineService(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<AirlineModel>> GetAllAirlines()
        {
            return await _databaseContext.Airlines.Find(airline => true).ToListAsync();
        }

        public async Task<AirlineModel> GetAirlineById(string airlineId)
        {
            return await _databaseContext.Airlines.Find(airline => airline.AirlineId == airlineId).FirstOrDefaultAsync(); 
        }
    }
}
