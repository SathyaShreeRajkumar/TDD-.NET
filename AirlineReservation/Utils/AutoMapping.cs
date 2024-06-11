using AutoMapper;
using AirlineReservation.Models.Data;
using AirlineReservation.Models.Api;

namespace AirlineReservation.Utils
{
    public class AutoMapping : Profile
    {
        public AutoMapping() {
            CreateMap<AirlineDto, AirlineModel>();
        }
    }
}
