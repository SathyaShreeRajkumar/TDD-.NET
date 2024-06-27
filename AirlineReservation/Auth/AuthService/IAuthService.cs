using AirlineReservation.Models.Data;

namespace AirlineReservation.Auth.AuthService
{
    public interface IAuthService
    {
        Task<UserModel> GetUser(string username, string password);
        Task<string> GenerateJwtToken(UserModel user);
    }
}
