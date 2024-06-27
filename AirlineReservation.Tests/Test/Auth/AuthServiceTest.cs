using AirlineReservation.Auth.AuthService;
using AirlineReservation.Models.Data;
using AirlineReservation.Services.Database;
using Microsoft.Extensions.Configuration;
using AutoFixture;
using MongoDB.Driver;
using Moq;
using FluentAssertions;

namespace AirlineReservation.Tests.Test.Auth
{
    public class AuthServiceTest
    {
        private readonly Mock<IDatabaseContext> _mockDataBaseContext;
        private readonly Mock<IMongoCollection<UserModel>> _mockCollection;
        private readonly Mock<IAsyncCursor<UserModel>> _mockCursor;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Fixture _fixture = new Fixture();

        public AuthServiceTest() 
        {
            _mockDataBaseContext = new Mock<IDatabaseContext>();
            _mockCollection = new Mock<IMongoCollection<UserModel>>();
            _mockCursor = new Mock<IAsyncCursor<UserModel>>();
            _mockConfiguration = new Mock<IConfiguration>();
        }

        [Fact]
        public async Task GetUser_ReturnsUser()
        {
            var mockUser = _fixture.Create<UserModel>();
            var userName = _fixture.Create<string>();
            var password = _fixture.Create<string>();

            _mockCursor.SetupSequence(_ => _.MoveNextAsync(default))
               .ReturnsAsync(true)
               .ReturnsAsync(false);
            _mockCursor.Setup(_ => _.Current).Returns(new List<UserModel> { mockUser });

            _mockCollection.Setup(collection => collection.FindAsync(
                It.IsAny<FilterDefinition<UserModel>>(),
                It.IsAny<FindOptions<UserModel, UserModel>>(),
                default))
                .ReturnsAsync(_mockCursor.Object);

            _mockDataBaseContext.Setup(user => user.Users).Returns(_mockCollection.Object);

            var authService = new AuthService(_mockDataBaseContext.Object, _mockConfiguration.Object);

            var result = await authService.GetUser(userName,password);

            result.Should().BeEquivalentTo(mockUser);

        }
    }
}
