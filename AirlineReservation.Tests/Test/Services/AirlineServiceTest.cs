using AirlineReservation.Models.Data;
using AirlineReservation.Services.Airline;
using AirlineReservation.Services.Database;
using AutoFixture;
using FluentAssertions;
using MongoDB.Driver;
using Moq;

namespace AirlineReservation.Tests.Test.Services
{
    public class AirlineServiceTest
    {
        private readonly Mock<IDatabaseContext> _mockDataBaseContext;
        private readonly Fixture _fixture = new Fixture();

        public AirlineServiceTest()
        {
            _mockDataBaseContext = new Mock<IDatabaseContext>();
        }

        [Fact]
        public async Task GetAllAirlines_ReturnsListOfAirlines()
        {
            var mockAirlines = _fixture.CreateMany<AirlineModel>(); 
            var mockCursor = new Mock<IAsyncCursor<AirlineModel>>();
            var mockCollection = new Mock<IMongoCollection<AirlineModel>>();

            mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                .ReturnsAsync(true)
                .ReturnsAsync(false);
            mockCursor.Setup(_ => _.Current).Returns(mockAirlines);

            mockCollection.Setup(collection => collection.FindAsync(
                It.IsAny<FilterDefinition<AirlineModel>>(),
                It.IsAny<FindOptions<AirlineModel, AirlineModel>>(),
                default))
                .ReturnsAsync(mockCursor.Object);

            _mockDataBaseContext.Setup(airline => airline.Airlines).Returns(mockCollection.Object);

            var airlineService = new AirlineService(_mockDataBaseContext.Object);

            var result = await airlineService.GetAllAirlines();

            result.Should().BeEquivalentTo(mockAirlines);
        }

        [Fact]
        public async Task GetAirlineById_ReturnsAirline_ByTheId()
        {
            var mockAirlines = _fixture.Create<AirlineModel>();
            var mockCursor = new Mock<IAsyncCursor<AirlineModel>>();
            var mockCollection = new Mock<IMongoCollection<AirlineModel>>();
            var mockId = _fixture.Create<string>();

            mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                .ReturnsAsync(true)
                .ReturnsAsync(false);
            mockCursor.Setup(_ => _.Current).Returns(new List<AirlineModel> { mockAirlines });

            mockCollection.Setup(collection => collection.FindAsync(
                It.IsAny<FilterDefinition<AirlineModel>>(),
                It.IsAny<FindOptions<AirlineModel, AirlineModel>>(),
                default))
                .ReturnsAsync(mockCursor.Object);

            _mockDataBaseContext.Setup(airline => airline.Airlines).Returns(mockCollection.Object);

            var airlineService = new AirlineService(_mockDataBaseContext.Object);

            var result = await airlineService.GetAirlineById(mockId);

            result.Should().BeEquivalentTo(mockAirlines);
        }
    }
}
