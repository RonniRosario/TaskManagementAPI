using Microsoft.AspNetCore.Mvc;
using TasksAPI.Login;
using TasksAPI.Models;

namespace TasksAPI.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ActionResult> CreateUser(Usuario user);
        Task<ActionResult> Login(LoginDTO login);
        Task<ActionResult> RefreshToken(string refreshToken);
    }
}
