﻿using AirlineReservation.Models.Api;
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
        private readonly Fixture _fixture = new Fixture();

        public AirlineServiceTest()
        {
            _mockMapper = new Mock<IMapper>();
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

            var airlineService = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);

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

            var airlineService = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);

            var result = await airlineService.GetAirlineById(mockId);

            result.Should().BeEquivalentTo(mockAirlines);
        }

        [Fact]
        public async Task CreateAirline_Should_InsertAirline()
        {
            var mockAirineDto = _fixture.Create<AirlineDto>();
            var mockAirlines = _fixture.Create<AirlineModel>();
            var mockCollection = new Mock<IMongoCollection<AirlineModel>>();

            _mockMapper.Setup(mapper => mapper.Map<AirlineModel>(mockAirineDto)).Returns(mockAirlines);
            _mockDataBaseContext.Setup(database => database.Airlines).Returns(mockCollection.Object);
            mockCollection.Setup(collection => collection.InsertOneAsync(mockAirlines, null, default))
                  .Returns(Task.CompletedTask);

            var airlineService = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);
            var result = await airlineService.CreateAirline(mockAirineDto);

            mockCollection.Verify(collection => collection.InsertOneAsync(mockAirlines, null, default), Times.Once());

            result.Should().BeEquivalentTo(mockAirlines);
        }

        [Fact]
        public async Task DeleteAirline_Should_DeleteAirline()
        {
            var message = _fixture.Create<int>();
            var mockId = _fixture.Create<string>();
            var mockCollection = new Mock<IMongoCollection<AirlineModel>>();

            _mockDataBaseContext.Setup(database => database.Airlines).Returns(mockCollection.Object);

            var deleteResult = new DeleteResult.Acknowledged(message);
            mockCollection.Setup(collection => collection.DeleteOneAsync(It.IsAny<FilterDefinition<AirlineModel>>(), default))
                          .ReturnsAsync(deleteResult);

            var airlineService = new AirlineService(_mockMapper.Object, _mockDataBaseContext.Object);

            var result = await airlineService.DeleteAirline(mockId);

            mockCollection.Verify(collection => collection.DeleteOneAsync(It.IsAny<FilterDefinition<AirlineModel>>(), default), Times.Once());
            Assert.Equal(message, result);
        }
    }
}