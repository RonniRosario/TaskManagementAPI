using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksAPI.DB;
using TasksAPI.JWT;
using TasksAPI.Models;

namespace TasksAPI.Services.Usuarios
{
    public class UsuariosServices: ICrudServices<Usuario>
    {
        private readonly TaskAPIContext _context;
        private readonly Utilities _utilities;
        public UsuariosServices(TaskAPIContext context, Utilities utilities)
        {
            _context = context;
            _utilities = utilities;
        }

        public async Task<ActionResult> Create(Usuario item)
        {
            try
            {
                //Crear el usuario y encriptar la contraseña
                var newUser = new Usuario
                {
                    Nombre = item.Nombre,
                    Email = item.Email,
                    FechaNacimiento = item.FechaNacimiento,
                    password = _utilities.encryptSHA256(item.password)
                };
                
                await _context.Usuarios.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return new OkObjectResult("El usuario ha sido creado exitosamente");
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userFound = await _context.Usuarios.FindAsync(id);
            if (userFound == null) { return new NotFoundObjectResult("No se encontro el usuario"); }

            _context.Usuarios.Remove(userFound);

            await _context.SaveChangesAsync();
            return new OkObjectResult("Se borro el usuario");
        }

        public async Task<ActionResult<Usuario>> Get(int id)
        {
            var userId = await _context.Usuarios.FindAsync(id);
            if (userId == null) { return new NotFoundObjectResult("No se encontro el usuario"); }

            return new OkObjectResult(userId);
        }

        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll() =>
            await _context.Usuarios.ToListAsync();



        public async Task<IActionResult> Update(int id, Usuario item)
        {
            var userFound = await _context.Usuarios.FindAsync(id);
            if (userFound == null) { return new NotFoundObjectResult("No se encontro el usuario"); }
            

            userFound.Nombre = item.Nombre;
            userFound.Email = item.Email;
            userFound.FechaNacimiento = item.FechaNacimiento;
            userFound.password = item.password;
            
            await _context.SaveChangesAsync();

            return new OkObjectResult("Se edito el usuario");
        }
    }
}
