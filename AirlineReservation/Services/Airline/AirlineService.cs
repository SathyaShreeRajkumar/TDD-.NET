using AirlineReservation.Models.Data;
using AirlineReservation.Services.Database;
using AutoMapper;
using MongoDB.Driver;

namespace AirlineReservation.Services.Airline
{
    public class AirlineService : IAirlineService
    {
        private IMapper _mapper;
        private IDatabaseContext _databaseContext;

        public AirlineService(IMapper mapper, IDatabaseContext databaseContext)
        {
            _mapper = mapper;
            _databaseContext = databaseContext;
        }
        public async Task<List<AirlineModel>> getAllAirlines()
        {
            return await _databaseContext.Airlines.Find(airline => true).ToListAsync();
        }
    }
}
