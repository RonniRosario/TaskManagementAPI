using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Login;
using TasksAPI.Models;
using TasksAPI.Services.AuthServices;

namespace TasksAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registrar")]
        public Task<ActionResult> CreateUser(Usuario user)
            => _authService.CreateUser(user);

        [HttpPost("login")]
        public Task<ActionResult> Login(LoginDTO user)
            => _authService.Login(user);

        [HttpPost("refresh")]
        public Task<ActionResult> RefreshToken([FromBody] string refreshToken)
            => _authService.RefreshToken(refreshToken);
    }
}
