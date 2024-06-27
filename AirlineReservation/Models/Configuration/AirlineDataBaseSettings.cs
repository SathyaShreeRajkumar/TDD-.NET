namespace AirlineReservation.Models.Configuration
{
    public class AirlineDataBaseSettings
    {
        public string ConnectionUrl { get; set; }
        public string DatabaseName { get; set; }
        public string AirlineCollectionName { get; set; }
        public string UserCollectionName { get; set; }
    }
}
