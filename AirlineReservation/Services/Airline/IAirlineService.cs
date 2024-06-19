﻿using AirlineReservation.Models.Api;
using AirlineReservation.Models.Data;

namespace AirlineReservation.Services.Airline
{
    public interface IAirlineService
    {
        Task<List<AirlineModel>> GetAllAirlines();

        Task<AirlineModel> GetAirlineById(string id);

        Task<AirlineModel> CreateAirline(AirlineDto airline);

        Task<AirlineModel> UpdateAirline(string id, AirlineDto airline);

        Task<long> DeleteAirline(string id);

        Task<List<AirlineModel>> SearchByBoarding(string boarding);

        Task<List<AirlineModel>> SearchByDestination(string destination);

        Task<List<AirlineModel>> SearchByAirline(string name);
    }
}