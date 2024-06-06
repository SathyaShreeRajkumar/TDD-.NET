namespace AirlineReservation.Models.Api
{
    public record AirlineDto
    {

        public string Name { get; set; }
        public string Boarding { get; set; }
        public string Destination { get; set; }
    }
}
