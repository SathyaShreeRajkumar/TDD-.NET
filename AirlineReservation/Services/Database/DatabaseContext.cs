using AirlineReservation.Models.Configuration;
using AirlineReservation.Models.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AirlineReservation.Services.Database
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly IMongoCollection<AirlineModel> _airline;
        public DatabaseContext(IOptions<AirlineDataBaseSettings> options, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(options.Value.DatabaseName);
            _airline = database.GetCollection<AirlineModel>(options.Value.CollectionName);

        }

        public IMongoCollection<AirlineModel> Airlines => _airline;
    }
}
