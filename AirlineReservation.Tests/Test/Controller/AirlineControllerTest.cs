using Airline_Reservation.Models.Data;
using Airline_Reservation.Services.Airline;
using AirlineReservation.Controllers;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;

namespace AirlineReservation.Tests.Test.Controller
{
    public class VehicleControllerTest
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public async Task ShouldReturnOn_GetAllVehicle_AndStatusAs200()
        {
            var mockVehicleService = new Mock<IAirlineService>();
            var mockVehicle = _fixture.Create<List<AirlineModel>>();
            mockVehicleService
                .Setup(service => service.getAllAirlines())
                .ReturnsAsync(mockVehicle);

            var testController = new AirlineController(mockVehicleService.Object);
            var result = await testController.GetAllAirlines();

            mockVehicleService.Verify(service => service.getAllAirlines(), Times.Once());

            result
                .Should().BeOfType<OkObjectResult>().Which.Value.Should()
                .BeOfType<List<AirlineModel>>().And.Subject.As<List<AirlineModel>>()
                .Should().BeEquivalentTo(mockVehicle);

            result
                .Should().BeOfType<OkObjectResult>().Which.StatusCode.Should()
                .Be(StatusCodes.Status200OK);


        }
    }
}
