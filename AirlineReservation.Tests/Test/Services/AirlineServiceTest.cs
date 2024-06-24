using AirlineReservation.Models.Api;
using AirlineReservation.Models.Data;
using AirlineReservation.Services.Airline;
using AirlineReservation.Services.Database;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MongoDB.Driver;
using Moq;

namespace AirlineReservation.Tests.Test.Services
{
    public class AirlineServiceTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IDatabaseContext> _mockDataBaseContext;
        private readonly Mock<IMongoCollection<AirlineModel>> _mockCollection;
        private readonly Mock<IAsyncCursor<AirlineModel>> _mockCursor;
        private readonly Fixture _fixture = new Fixture();

        public AirlineServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockDataBaseContext = new Mock<IDatabaseContext>();
            _mockCollection = new Mock<IMongoCollection<AirlineModel>>();
            _mockCursor = new Mock<IAsyncCursor<AirlineModel>>();
        }

        [Fact]
        public async Task GetAllAirlines_ReturnsListOfAirlines()
        {
            var mockAirlines = _fixture.CreateMany<AirlineModel>();

           _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                .ReturnsAsync(true)
                .ReturnsAsync(false);
            _mockCursor.Setup(_ => _.Current).Returns(mockAirlines);

            _mockCollection.Setup(collection => collection.FindAsync(
                It.IsAny<FilterDefinition<AirlineModel>>(),
                It.IsAny<FindOptions<AirlineModel, AirlineModel>>(),
                default))
                .ReturnsAsync(_mockCursor.Object);

            _mockDataBaseContext.Setup(airline => airline.Airlines).Returns(_mockCollection.Object);

            var airlineService = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);

            var result = await airlineService.GetAllAirlines();

            result.Should().BeEquivalentTo(mockAirlines);
        }

        [Fact]
        public async Task GetAirlineById_ReturnsAirline_ByTheId()
        {
            var mockAirlines = _fixture.Create<AirlineModel>();
            var mockId = _fixture.Create<string>();

            _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                .ReturnsAsync(true)
                .ReturnsAsync(false);
            _mockCursor.Setup(_ => _.Current).Returns(new List<AirlineModel> { mockAirlines });

            _mockCollection.Setup(collection => collection.FindAsync(
                It.IsAny<FilterDefinition<AirlineModel>>(),
                It.IsAny<FindOptions<AirlineModel, AirlineModel>>(),
                default))
                .ReturnsAsync(_mockCursor.Object);

            _mockDataBaseContext.Setup(airline => airline.Airlines).Returns(_mockCollection.Object);

            var airlineService = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);

            var result = await airlineService.GetAirlineById(mockId);

            result.Should().BeEquivalentTo(mockAirlines);
        }

        [Fact]
        public async Task CreateAirline_Should_InsertAirline()
        {
            var mockAirineDto = _fixture.Create<AirlineDto>();
            var mockAirlines = _fixture.Create<AirlineModel>();

            _mockMapper.Setup(mapper => mapper.Map<AirlineModel>(mockAirineDto)).Returns(mockAirlines);
            _mockDataBaseContext.Setup(database => database.Airlines).Returns(_mockCollection.Object);
            _mockCollection.Setup(collection => collection.InsertOneAsync(mockAirlines, null, default))
                  .Returns(Task.CompletedTask);

            var airlineService = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);
            var result = await airlineService.CreateAirline(mockAirineDto);

            _mockCollection.Verify(collection => collection.InsertOneAsync(mockAirlines, null, default), Times.Once());

            result.Should().BeEquivalentTo(mockAirlines);
        }

        [Fact]
        public async Task DeleteAirline_Should_DeleteAirline()
        {
            var message = _fixture.Create<int>();
            var mockId = _fixture.Create<string>();

            _mockDataBaseContext.Setup(database => database.Airlines).Returns(_mockCollection.Object);

            var deleteResult = new DeleteResult.Acknowledged(message);
            _mockCollection.Setup(collection => collection.DeleteOneAsync(It.IsAny<FilterDefinition<AirlineModel>>(), default))
                          .ReturnsAsync(deleteResult);

            var airlineService = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);

            var result = await airlineService.DeleteAirline(mockId);

            _mockCollection.Verify(collection => collection.DeleteOneAsync(It.IsAny<FilterDefinition<AirlineModel>>(), default), Times.Once());
            Assert.Equal(message, result);
        }

        [Fact]
        public async Task UpdateAirline_ShouldReturn_UpdatedAirline()
        {
            var mockId = _fixture.Create<string>();
            var mockAirlineDto = _fixture.Create<AirlineDto>();
            var mockAirline = _fixture.Create<AirlineModel>();
            mockAirline.AirlineId =mockId;


            _mockCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true)
                      .ReturnsAsync(false);
            _mockCursor.Setup(_ => _.Current).Returns(new List<AirlineModel> { mockAirline });

            _mockCollection.Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<AirlineModel>>(),
                It.IsAny<FindOptions<AirlineModel, AirlineModel>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockCursor.Object);

            _mockCollection.Setup(x => x.ReplaceOneAsync(
                It.IsAny<FilterDefinition<AirlineModel>>(),
                It.IsAny<AirlineModel>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReplaceOneResult.Acknowledged(1, 1, null));

            _mockDataBaseContext.Setup(database => database.Airlines)
                                .Returns(_mockCollection.Object);

            _mockMapper.Setup(mapper => mapper.Map(mockAirlineDto, mockAirline));

            var airlineService = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);

            var result = await airlineService.UpdateAirline(mockId, mockAirlineDto);

            result.Should().BeEquivalentTo(mockAirline);
        }


        [Fact]
        public async Task SearchByAirline_ReturnsAirlines()
        {
            var mockAirlines = _fixture.CreateMany<AirlineModel>();
            var mockName = mockAirlines.First().Name;
            var mockBoarding = mockAirlines.First().Boarding;
            var mockDestination = mockAirlines.First().Destination;

            _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
                .ReturnsAsync(true)
                .ReturnsAsync(false);
            _mockCursor.Setup(_ => _.Current).Returns(mockAirlines);

            _mockCollection.Setup(collection => collection.FindAsync(
                It.IsAny<FilterDefinition<AirlineModel>>(),
                It.IsAny<FindOptions<AirlineModel, AirlineModel>>(),
                default))
                .ReturnsAsync(_mockCursor.Object);

            _mockDataBaseContext.Setup(airline => airline.Airlines).Returns(_mockCollection.Object);

            var airlineService = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);

            var result = await airlineService.SearchAirline(mockName,mockBoarding,mockDestination);

            result.Should().BeEquivalentTo(mockAirlines);
        }

    }
}
