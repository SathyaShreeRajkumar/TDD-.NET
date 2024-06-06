using AirlineReservation.Models.Data;
using AirlineReservation.Services.Airline;
using AirlineReservation.Controllers;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

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

            mockAirlinesService.Verify(service => service.GetAllAirlines(), Times.Once);

            result
                .Should().BeOfType<OkObjectResult>().Which.Value.Should()
                .BeOfType<List<AirlineModel>>().And.Subject.As<List<AirlineModel>>()
                .Should().BeEquivalentTo(mockAirlines);

            result.Should().BeOfType<OkObjectResult>();
        }

    }
}
