using AirlineReservation.Models.Configuration;
using AirlineReservation.Models.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AirlineReservation.Services.Database
{
    public class DatabaseContext : IDatabaseContext
    {
        private readonly IMongoCollection<AirlineModel> _airline;
        private readonly IMongoCollection<UserModel> _user;
        public DatabaseContext(IOptions<AirlineDataBaseSettings> options, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(options.Value.DatabaseName);
            _airline = database.GetCollection<AirlineModel>(options.Value.AirlineCollectionName);
            _user = database.GetCollection<UserModel>(options.Value.UserCollectionName);

        }

        public IMongoCollection<AirlineModel> Airlines => _airline;
        public IMongoCollection <UserModel> Users => _user;
    }
}
