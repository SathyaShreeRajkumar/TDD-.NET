using AirlineReservation.Models.Api;
using AirlineReservation.Models.Data;
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
