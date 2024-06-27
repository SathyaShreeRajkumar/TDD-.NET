using AirlineReservation.Auth.AuthService;
using AirlineReservation.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservation.Auth.AuthController
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("generateToken")]
        public async Task<IActionResult> CreateToken(UserModel user)
        {
            var validUser = await _authService.GetUser(user.UserName, user.Password);

            if (validUser == null)
            {
                return Unauthorized();
            }

            var token = await _authService.GenerateJwtToken(validUser);

            return Ok(new { token });
        }
    }
}

