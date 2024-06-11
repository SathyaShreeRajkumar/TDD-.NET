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
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task ShouldReturnOn_GetAllAirlines_AndStatusAs200()
        {
            var mockAirlinesService = new Mock<IAirlineService>();
            var mockAirlines = _fixture.Create<List<AirlineModel>>();
            mockAirlinesService.Setup(service => service.GetAllAirlines()).ReturnsAsync(mockAirlines);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.GetAllAirlines();

            mockAirlinesService.Verify(service => service.GetAllAirlines(), Times.Once());

            result
                .Should().BeOfType<OkObjectResult>().Which.Value
                .Should().BeEquivalentTo(mockAirlines);
        }

        [Fact]
        public async Task ShouldReturnOn_GetAirlineById_AndStatusAs200()
        {
            var mockAirlinesService = new Mock<IAirlineService>();
            var mockAirline = _fixture.Create<AirlineModel>();
            var mockId = _fixture.Create<string>();

            mockAirlinesService.Setup(service => service.GetAirlineById(mockId)).ReturnsAsync(mockAirline);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.GetAirlineById(mockId);

            mockAirlinesService.Verify(service => service.GetAirlineById(mockId), Times.Once());

            result
                .Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(mockAirline);
        }

        [Fact]
        public async Task ShouldReturnOn_AirlineNotFoundForId_AndStatusAs404()
        {
            var mockAirlinesService = new Mock<IAirlineService>();
            var mockId = _fixture.Create<string>();

            mockAirlinesService.Setup(service => service.GetAirlineById(mockId))
                           .ReturnsAsync(null as AirlineModel);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.GetAirlineById(mockId);

            mockAirlinesService.Verify(service => service.GetAirlineById(mockId), Times.Once());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ShouldReturnOn_CreationOfAirlineObject_AndStatusAs201()
        {
            var mockAirline = _fixture.Create<AirlineModel>();
            var mockAirlineDto = _fixture.Create<AirlineDto>();
            var mockAirlinesService = new Mock<IAirlineService>();

            mockAirlinesService.Setup(service => service.CreateAirline(mockAirlineDto))
                               .ReturnsAsync(mockAirline);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.CreateAirline(mockAirlineDto);

            mockAirlinesService.Verify(service => service.CreateAirline(mockAirlineDto), Times.Once());

            result
                .Should().BeOfType<CreatedAtActionResult>().Which.Value.Should()
                .Be(mockAirline);
        }

        [Fact]
        public async Task ShouldReturnOn_InvalidObject_AndStatusAs404()
        {
            var mockAirline = _fixture.Create<AirlineModel>();
            var mockAirlineDto = _fixture.Create<AirlineDto>();
            var mockAirlinesService = new Mock<IAirlineService>();

            mockAirlinesService.Setup(service => service.CreateAirline(mockAirlineDto))
                             .ReturnsAsync(null as AirlineModel);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.CreateAirline(mockAirlineDto);

            mockAirlinesService.Verify(service => service.CreateAirline(mockAirlineDto), Times.Once());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ShouldReturnOn_DeleteAirline_AndStatusAs200()
        {
            var mockAirlineId = _fixture.Create<string>();
            var mockAirlinesService = new Mock<IAirlineService>();

            mockAirlinesService.Setup(service => service.DeleteAirline(mockAirlineId)).ReturnsAsync(10);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.DeleteAirline(mockAirlineId);

            mockAirlinesService.Verify(service => service.DeleteAirline(mockAirlineId), Times.Once());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task ShouldReturnOn_IdNotFoundForDelete_AndStatusAs404()
        {
            var mockAirlineId = _fixture.Create<string>();
            var mockAirlinesService = new Mock<IAirlineService>();

            mockAirlinesService.Setup(service => service.DeleteAirline(mockAirlineId)).ReturnsAsync(0);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.DeleteAirline(mockAirlineId);

            mockAirlinesService.Verify(service => service.DeleteAirline(mockAirlineId), Times.Once());

            result.Should().BeOfType<NotFoundResult>();
        }

    }
}
