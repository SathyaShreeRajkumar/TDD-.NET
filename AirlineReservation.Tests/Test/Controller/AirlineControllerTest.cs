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

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(mockAirlines);
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

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(mockAirline);
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
        public async Task ShouldReturnOn_CreateAirlineObject_AndStatusAs201()
        {
            var mockAirline = _fixture.Create<AirlineModel>();
            var mockAirlineDto = _fixture.Create<AirlineDto>();
            var mockAirlinesService = new Mock<IAirlineService>();

            mockAirlinesService.Setup(service => service.CreateAirline(mockAirlineDto))
                               .ReturnsAsync(mockAirline);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.CreateAirline(mockAirlineDto);

            mockAirlinesService.Verify(service => service.CreateAirline(mockAirlineDto), Times.Once());

            result.Should().BeOfType<CreatedAtActionResult>()
                    .Which.Value.Should().BeEquivalentTo(mockAirline);
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

        [Fact] 
        public async Task ShouldReturnOn_UpdateAirlineObject_AndStatusAs202()
        {
            var mockAirlineId = _fixture.Create<string>();
            var mockAirlinesService = new Mock<IAirlineService>();
            var mockAirlineDto = _fixture.Create<AirlineDto>();
            var mockAirline = _fixture.Create<AirlineModel>();

            mockAirlinesService.Setup(service => service.UpdateAirline(mockAirlineId,mockAirlineDto))
                             .ReturnsAsync(mockAirline);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.UpdateAirline(mockAirlineId, mockAirlineDto);

            mockAirlinesService.Verify(service => service.UpdateAirline(mockAirlineId, mockAirlineDto), Times.Once());

            result.Should().BeOfType<AcceptedAtActionResult>()
                    .Which.Value.Should().BeEquivalentTo(mockAirline);

        }

        [Fact]
        public async Task ShouldReturnNotFoundWhen_IdNotFoundForUpdate()
        {
            var mockAirlineId = _fixture.Create<string>();
            var mockAirlineDto = _fixture.Create<AirlineDto>();
            var mockAirlinesService = new Mock<IAirlineService>();

            mockAirlinesService.Setup(service => service.UpdateAirline(mockAirlineId, mockAirlineDto))
                .ReturnsAsync(null as AirlineModel);

            var airlineController = new AirlineController(mockAirlinesService.Object);

            var result = await airlineController.UpdateAirline(mockAirlineId, mockAirlineDto);

            mockAirlinesService.Verify(service => service.UpdateAirline(mockAirlineId, mockAirlineDto), Times.Once());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ShouldReturnOn_SearchBoardingObject_AndStatusAs200()
        {
            var mockAirlinesService = new Mock<IAirlineService>();
            var mockAirline = _fixture.Create<List<AirlineModel>>();
            var mockBoarding = _fixture.Create<string>();

            mockAirlinesService.Setup(service => service.SearchByBoarding(mockBoarding)).ReturnsAsync(mockAirline);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.SearchAirlineByBoarding(mockBoarding);

            mockAirlinesService.Verify(service => service.SearchByBoarding(mockBoarding), Times.Once());

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(mockAirline);
        }

        [Fact]
        public async Task ShouldReturnOn_InvalidBoardingSearch_AndStatus404()
        {
            var mockAirlinesService = new Mock<IAirlineService>();
            var mockAirline = _fixture.Create<List<AirlineModel>>();
            string? inavlidBoarding = null;

            mockAirlinesService.Setup(service => service.SearchByBoarding(inavlidBoarding)).ReturnsAsync(null as List<AirlineModel>);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.SearchAirlineByBoarding(inavlidBoarding);

            mockAirlinesService.Verify(service => service.SearchByBoarding(inavlidBoarding), Times.Once());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ShouldReturnOn_SearchDestinationObject_AndStatusAs200()
        {
            var mockAirlinesService = new Mock<IAirlineService>();
            var mockAirline = _fixture.Create<List<AirlineModel>>();
            var mockDestination = _fixture.Create<string>();

            mockAirlinesService.Setup(service => service.SearchByDestination(mockDestination)).ReturnsAsync(mockAirline);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.SearchAirlineByDestination(mockDestination);

            mockAirlinesService.Verify(service => service.SearchByDestination(mockDestination), Times.Once());

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(mockAirline);
        }

        [Fact]
        public async Task ShouldReturnOn_InvalidDestinationSearch_AndStatus404()
        {
            var mockAirlinesService = new Mock<IAirlineService>();
            var mockAirline = _fixture.Create<List<AirlineModel>>();
            string? invalidDestination = null;

            mockAirlinesService.Setup(service => service.SearchByDestination(invalidDestination)).ReturnsAsync(null as List<AirlineModel>);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.SearchAirlineByDestination(invalidDestination);

            mockAirlinesService.Verify(service => service.SearchByDestination(invalidDestination), Times.Once());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ShouldReturnOn_SearchAirlineObject_AndStatusAs200()
        {
            var mockAirlinesService = new Mock<IAirlineService>();
            var mockAirline = _fixture.Create<List<AirlineModel>>();
            var mockAirlineName = _fixture.Create<string>();

            mockAirlinesService.Setup(service => service.SearchByAirline(mockAirlineName)).ReturnsAsync(mockAirline);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.SearchByAirline(mockAirlineName);

            mockAirlinesService.Verify(service => service.SearchByAirline(mockAirlineName), Times.Once());

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(mockAirline);
        }

        [Fact]
        public async Task ShouldReturnOn_InvalidAirlineSearch_AndStatus404()
        {
            var mockAirlinesService = new Mock<IAirlineService>();
            var mockAirline = _fixture.Create<List<AirlineModel>>();
            string? invalidAirline = null;

            mockAirlinesService.Setup(service => service.SearchByAirline(invalidAirline)).ReturnsAsync(null as List<AirlineModel>);

            var airlineController = new AirlineController(mockAirlinesService.Object);
            var result = await airlineController.SearchByAirline(invalidAirline);

            mockAirlinesService.Verify(service => service.SearchByAirline(invalidAirline), Times.Once());

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
