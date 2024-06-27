using AirlineReservation.Models.Data;
using AirlineReservation.Services.Airline;
using AirlineReservation.Controllers;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AirlineReservation.Models.Api;

namespace AirlineReservation.Tests.Test.Controller
{
    public class AirlineControllerTest
    {
        private readonly Mock<IAirlineService> _mockAirlinesService;
        private readonly AirlineController _airlineController;
        private readonly Fixture _fixture = new Fixture();

        public AirlineControllerTest()
        {
            _mockAirlinesService = new Mock<IAirlineService>();
            _airlineController = new AirlineController(_mockAirlinesService.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ShouldReturnOn_GetAllAirlines_AndStatusAs200()
        {
            var mockAirlines = _fixture.Create<List<AirlineModel>>();
            _mockAirlinesService.Setup(service => service.GetAllAirlines()).ReturnsAsync(mockAirlines);

            var result = await _airlineController.GetAllAirlines();

            _mockAirlinesService.Verify(service => service.GetAllAirlines(), Times.Once());

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(mockAirlines);
        }

        [Fact]
        public async Task ShouldReturnOn_GetAirlineById_AndStatusAs200()
        {
            var mockAirline = _fixture.Create<AirlineModel>();
            var mockId = _fixture.Create<string>();

            _mockAirlinesService.Setup(service => service.GetAirlineById(mockId)).ReturnsAsync(mockAirline);

            var result = await _airlineController.GetAirlineById(mockId);

            _mockAirlinesService.Verify(service => service.GetAirlineById(mockId), Times.Once());

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(mockAirline);
        }

        [Fact]
        public async Task ShouldReturnOn_AirlineNotFoundForId_AndStatusAs404()
        {
            var mockId = _fixture.Create<string>();

            _mockAirlinesService.Setup(service => service.GetAirlineById(mockId))
                           .ReturnsAsync(null as AirlineModel);

            var result = await _airlineController.GetAirlineById(mockId);

            _mockAirlinesService.Verify(service => service.GetAirlineById(mockId), Times.Once());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ShouldReturnOn_CreateAirlineObject_AndStatusAs201()
        {
            var mockAirline = _fixture.Create<AirlineModel>();
            var mockAirlineDto = _fixture.Create<AirlineDto>();

            _mockAirlinesService.Setup(service => service.CreateAirline(mockAirlineDto))
                               .ReturnsAsync(mockAirline);

            var result = await _airlineController.CreateAirline(mockAirlineDto);

            _mockAirlinesService.Verify(service => service.CreateAirline(mockAirlineDto), Times.Once());

            result.Should().BeOfType<CreatedAtActionResult>()
                    .Which.Value.Should().BeEquivalentTo(mockAirline);
        }

        [Fact]
        public async Task ShouldReturnOn_InvalidObject_AndStatusAs404()
        {
            var mockAirline = _fixture.Create<AirlineModel>();
            var mockAirlineDto = _fixture.Create<AirlineDto>();

            _mockAirlinesService.Setup(service => service.CreateAirline(mockAirlineDto))
                             .ReturnsAsync(null as AirlineModel);

            var result = await _airlineController.CreateAirline(mockAirlineDto);

            _mockAirlinesService.Verify(service => service.CreateAirline(mockAirlineDto), Times.Once());

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task ShouldReturnOn_DeleteAirline_AndStatusAs200()
        {
            var mockAirlineId = _fixture.Create<string>();

            _mockAirlinesService.Setup(service => service.DeleteAirline(mockAirlineId)).ReturnsAsync(10);

            var result = await _airlineController.DeleteAirline(mockAirlineId);

            _mockAirlinesService.Verify(service => service.DeleteAirline(mockAirlineId), Times.Once());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnOn_IdNotFoundForDelete_AndStatusAs404()
        {
            var mockAirlineId = _fixture.Create<string>();

            _mockAirlinesService.Setup(service => service.DeleteAirline(mockAirlineId)).ReturnsAsync(0);

            var result = await _airlineController.DeleteAirline(mockAirlineId);

            _mockAirlinesService.Verify(service => service.DeleteAirline(mockAirlineId), Times.Once());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact] 
        public async Task ShouldReturnOn_UpdateAirlineObject_AndStatusAs202()
        {
            var mockAirlineId = _fixture.Create<string>();
            var mockAirlineDto = _fixture.Create<AirlineDto>();
            var mockAirline = _fixture.Create<AirlineModel>();

            _mockAirlinesService.Setup(service => service.UpdateAirline(mockAirlineId,mockAirlineDto))
                             .ReturnsAsync(mockAirline);

            var result = await _airlineController.UpdateAirline(mockAirlineId, mockAirlineDto);

            _mockAirlinesService.Verify(service => service.UpdateAirline(mockAirlineId, mockAirlineDto), Times.Once());

            result.Should().BeOfType<AcceptedAtActionResult>()
                    .Which.Value.Should().BeEquivalentTo(mockAirline);

        }

        [Fact]
        public async Task ShouldReturnNotFoundWhen_IdNotFoundForUpdate()
        {
            var mockAirlineId = _fixture.Create<string>();
            var mockAirlineDto = _fixture.Create<AirlineDto>();

            _mockAirlinesService.Setup(service => service.UpdateAirline(mockAirlineId, mockAirlineDto))
                .ReturnsAsync(null as AirlineModel);

            var result = await _airlineController.UpdateAirline(mockAirlineId, mockAirlineDto);

            _mockAirlinesService.Verify(service => service.UpdateAirline(mockAirlineId, mockAirlineDto), Times.Once());

            result.Should().BeOfType<BadRequestResult>();
        }


        [Fact]
        public async Task ShouldReturnOn_SearchAirlineObject_AndStatusAs200()
        {
            var mockAirline = _fixture.Create<List<AirlineModel>>();
            var mockName = _fixture.Create<string>();
            var mockBoarding = _fixture.Create<string> ();
            var mockDestination = _fixture.Create<string>();

            _mockAirlinesService.Setup(service => service.SearchAirline(mockName,mockBoarding,mockDestination)).ReturnsAsync(mockAirline);

            var result = await _airlineController.SearchAirlines(mockName, mockBoarding, mockDestination);

            _mockAirlinesService.Verify(service => service.SearchAirline(mockName, mockBoarding, mockDestination), Times.Once());

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(mockAirline);
        }

        [Fact]
        public async Task ShouldReturnOn_InvalidAirlineSearch_AndStatus404()
        {
            var invalidName = _fixture.Create<string>();
            var invalidBoarding = _fixture.Create<string>();
            var invalidDestination = _fixture.Create<string>();

            _mockAirlinesService.Setup(service => service.SearchAirline(invalidName,invalidBoarding,invalidDestination)).ReturnsAsync(null as List<AirlineModel>);

            var result = await _airlineController.SearchAirlines(invalidName, invalidBoarding, invalidDestination);

            _mockAirlinesService.Verify(service => service.SearchAirline(invalidName, invalidBoarding, invalidDestination), Times.Once());

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
