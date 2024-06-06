using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AirlineReservation.Models.Data
{
    public record AirlineModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string AirlineId { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("boarding")]
        public string Boarding { get; set; }

        [BsonElement("destination")]
        public string Destination { get; set; }
    }
}
