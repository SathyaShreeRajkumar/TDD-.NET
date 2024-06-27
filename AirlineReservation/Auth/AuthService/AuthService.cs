using AirlineReservation.Models.Data;
using AirlineReservation.Services.Database;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AirlineReservation.Auth.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IMongoCollection<UserModel> _users;
        private readonly IConfiguration _configuration;

        public AuthService(IDatabaseContext databaseContext, IConfiguration configuration)
        {
            _users = databaseContext.Users;
            _configuration = configuration;
        }

        public async Task<UserModel> GetUser(string username, string password)
        {
            var filter = Builders<UserModel>.Filter.And(
                Builders<UserModel>.Filter.Eq(u => u.UserName, username),
                Builders<UserModel>.Filter.Eq(u => u.Password, password)
            );

            return await _users.Find(filter).FirstOrDefaultAsync();
        }

        public Task<string> GenerateJwtToken(UserModel user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}

