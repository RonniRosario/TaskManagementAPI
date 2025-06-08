using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TasksAPI.Models;
using TasksAPI.Services;

namespace TasksAPI.Controllers.Usuarios
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ICrudServices<Usuario> _services;

        public UsuariosController(ICrudServices<Usuario> services)
        {
            _services = services;
            
        }

        [HttpGet]
        [Route("Obtener")]
        public Task<ActionResult<IEnumerable<Usuario>>> Get()
            => _services.GetAll();

        [HttpGet]
        [Route("ObtenerId/{id}")]
        public Task<ActionResult<Usuario>> GetUser(int id)
            => _services.Get(id);

        [HttpPost]
        [Route("Crear")]
        public Task<ActionResult> CreateUser(Usuario user)
            => _services.Create(user);


        [HttpPut]
        [Route("Editar/{id}")]
        public Task<IActionResult> UpdateUser(int id, Usuario user)
            => _services.Update(id, user);

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public Task<IActionResult> DeleteUser(int id)
            => _services.Delete(id);
    }
}
