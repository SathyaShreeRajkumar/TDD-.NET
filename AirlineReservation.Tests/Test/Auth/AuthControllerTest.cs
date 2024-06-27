using AirlineReservation.Auth.AuthService;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AirlineReservation.Models.Data;
using AirlineReservation.Auth.AuthController;

namespace AirlineReservation.Tests.Test.Auth
{
    public class AuthControllerTest
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _authController;
        private readonly Fixture _fixture = new Fixture();

        public AuthControllerTest()
        {
            _mockAuthService = new Mock<IAuthService>();
            _authController = new AuthController(_mockAuthService.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task ShouldReturnOn_CreateToken_AndStatusAs200()
        {
            var mockUsers = _fixture.Create<UserModel>();
            var mockToken = _fixture.Create<string>();

            _mockAuthService.Setup(service => service.GetUser(mockUsers.UserName, mockUsers.Password))
                        .ReturnsAsync(mockUsers);

            _mockAuthService.Setup(service => service.GenerateJwtToken(mockUsers))
            .ReturnsAsync(mockToken);

            var result = await _authController.CreateToken(mockUsers);

            result.Should().BeOfType<OkObjectResult>()
                      .Which.Value.Should().BeEquivalentTo(new { token = mockToken });
        }

        [Fact]
        public async Task ShouldReturnOn_CreateTokenInvalid_AndStatusAs401()
        {
            var mockUsers = _fixture.Create<UserModel>();
            var mockToken = _fixture.Create<string>();

            _mockAuthService.Setup(service => service.GetUser(mockUsers.UserName, mockUsers.Password))
                        .ReturnsAsync(null as UserModel);

            _mockAuthService.Setup(service => service.GenerateJwtToken(mockUsers))
            .ReturnsAsync(mockToken);

            var result = await _authController.CreateToken(mockUsers);

            result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}
