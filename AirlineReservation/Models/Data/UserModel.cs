using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AirlineReservation.Models.Data
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }
    }
}
