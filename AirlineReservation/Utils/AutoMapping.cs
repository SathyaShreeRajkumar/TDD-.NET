using Airline_Reservation.Models.Api;
using Airline_Reservation.Models.Data;
using AutoMapper;

namespace AirlineReservation.Utils
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<AirlineModel, AirlineDto>();
        }
    }
}
