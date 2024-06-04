using Airline_Reservation.Models.Data;
using Airline_Reservation.Services.Airline;
using AirlineReservation.Services.Database;
using AutoFixture;
using AutoMapper;
using Moq;

namespace AirlineReservation.Tests.Test.Services
{
    public class AirlineServiceTest
    {
        private readonly Mock<IDatabaseContext> _mockDataBaseContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Fixture _fixture = new Fixture();

        public AirlineServiceTest()
        {
            _mockDataBaseContext = new Mock<IDatabaseContext>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task GetAllAirlines_Should_ReturnAllAirlines()
        {
            var mockAirlines = _fixture.Create<List<AirlineModel>>();
            var service = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);
            var result = await service.getAllAirlines();

            Assert.Equal(result, mockAirlines);
        }

    }
}
