using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksAPI.DB;
using TasksAPI.JWT;
using TasksAPI.Login;
using TasksAPI.Models;

namespace TasksAPI.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly TaskAPIContext _context;
        private readonly Utilities _utilities;
        private readonly RefreshTokenServices _refreshTokensServices;

        public AuthService(TaskAPIContext context, Utilities utilities, RefreshTokenServices refreshTokensServices)
        {
            _context = context;
            _utilities = utilities;
            _refreshTokensServices = refreshTokensServices;
        }
        public async Task<ActionResult> CreateUser(Usuario user)
        {
            try
            {
                var newUser = new Usuario
                {
                    Nombre = user.Nombre,
                    Email = user.Email,
                    FechaNacimiento = user.FechaNacimiento,
                    password = _utilities.encryptSHA256(user.password)
                };
                //var checkEmails = new Validations(_context);
                //if (await checkEmails.DuplicateEmails(user))
                //{
                //    Console.WriteLine("No pueden haber usuarios duplicados");
                //    return new BadRequestObjectResult("El correo ya esta en uso");

                //}
                await _context.Usuarios.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return new OkObjectResult("El usuario ha sido creado exitosamente");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Error: {ex.Message}");
            }
        }

        public async Task<ActionResult> Login(LoginDTO login)
        {
            var userFound = await _context.Usuarios.Where(
                                   u => u.Email == login.Email &&
                                   u.password == _utilities.encryptSHA256(login.password))
                                   .FirstOrDefaultAsync();

            if (userFound == null) { return new NotFoundObjectResult("No se encontro el usuario"); }

            string jwtToken = _utilities.generateJWT(userFound);
            string refreshToken = _refreshTokensServices.GenerateRefreshToken();


            await _refreshTokensServices.StoreRefreshTokenAsync(userFound.Id, refreshToken);

            return new OkObjectResult(new { Token = jwtToken, RefreshToken = refreshToken });
        }

        public async Task<ActionResult> RefreshToken(string refreshToken)
        {
            int? userId = await _refreshTokensServices.ValidateRefreshTokenAsync(refreshToken);

            if (!userId.HasValue)
            {
                return new UnauthorizedResult();
            }

            var user = await _context.Usuarios.FindAsync(userId.Value);
            if (user == null)
            {
                return new UnauthorizedResult();
            }

            string newJwtToken = _utilities.generateJWT(user);
            string newRefreshToken = _refreshTokensServices.GenerateRefreshToken();

            await _refreshTokensServices.StoreRefreshTokenAsync(user.Id, newRefreshToken);

            return new OkObjectResult(new { Token = newJwtToken, RefreshToken = newRefreshToken });
        }
    }
}
