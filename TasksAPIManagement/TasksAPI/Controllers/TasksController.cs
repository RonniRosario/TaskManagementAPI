using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TasksAPI.Delegates;
using TasksAPI.Hubs;
using TasksAPI.Models;
using TasksAPI.Services;
using TasksAPI.Services.Task;

namespace TasksAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ICrudServices<Tasks<int>> _services;
        private readonly ITasksService<Tasks<int>> _taskServices;
        
        
        public TasksController(ICrudServices<Tasks<int>> services, ITasksService<Tasks<int>> taskServices)
        {
             _services = services;
            _taskServices = taskServices;
            
            
        }

        [HttpGet]
        [Route("Obtener")]
        public Task<ActionResult<IEnumerable<Tasks<int>>>> GetAll() 
            =>_services.GetAll();

        [HttpGet]
        [Route("PorcentajeTareasCompletadas")]
        public Task<ActionResult<IEnumerable<Tasks<int>>>> CalculateTaskCompletionRate()
            => _taskServices.CalculateTaskCompletionRate();

        [HttpGet]
        [Route("ObtenerId/{id}")]
        public Task<ActionResult<Tasks<int>>> GetTask(int id)
            => _services.Get(id);

        [HttpGet]
        [Route("EstatusPendiente")]
        public Task<ActionResult<Tasks<int>>> GetTaskByStatusPending()
            => _taskServices.GetTaskByStatusPending();

        [HttpPost]
        [Route("Crear")]
        public Task<ActionResult> CreateTask(Tasks<int> task)
            => _services.Create(task);

        [HttpPost]
        [Route("CrearPrioridadAlta")]
        public Task<ActionResult> CreateHighPriorityTask(Tasks<int> task)
            => _taskServices.CreateHighPriorityTask(task);

        [HttpDelete]
        [Route("Eliminar/{id}")]

        public Task<IActionResult> DeleteTask(int id, Tasks<int> task)
            => _services.Delete(id);

        [HttpPut]
        [Route("Editar/{id}")]
        public Task<IActionResult> UpdateTask(int id, Tasks<int> task) 
            => _services.Update(id, task);

    }
}
