using AirlineReservation.Models.Api;
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

        public async Task<List<AirlineModel>> GetAllAirlines()
        {
            return await _databaseContext.Airlines.Find(airline => true).ToListAsync();
        }

        public async Task<AirlineModel> GetAirlineById(string airlineId)
        {
            return await _databaseContext.Airlines.Find(airline => airline.AirlineId == airlineId).FirstOrDefaultAsync();
        }

        public async Task<AirlineModel> CreateAirline(AirlineDto airlineDto)
        {
            var airlineData = _mapper.Map<AirlineModel>(airlineDto);

            await _databaseContext.Airlines.InsertOneAsync(airlineData);

            return airlineData;
        }

        public async Task<AirlineModel> UpdateAirline(string id,AirlineDto airlineDto)
        {
            var airlineData = await _databaseContext.Airlines.Find(airline => airline.AirlineId == id).FirstOrDefaultAsync();
            _mapper.Map(airlineDto , airlineData);

            var result = await _databaseContext.Airlines.ReplaceOneAsync(airline => airline.AirlineId == id, airlineData);

            return airlineData;
        }

        public async Task<long> DeleteAirline(string airlineId)
        {
            var airline = await _databaseContext.Airlines.DeleteOneAsync(airline => airline.AirlineId == airlineId);
            return airline.DeletedCount;
        }
    }
}
