using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;
using TasksAPI.DB;
using TasksAPI.Delegates;
using TasksAPI.Models;

namespace TasksAPI.Services
{
    public class TasksServices : ICrudServices<Tasks<int>>, ITasksService<Tasks<int>>
    {
        private readonly TaskAPIContext _context;
        private readonly TaskDelegates _delegates;
        private readonly TaskQueueService _queue;
        private static Dictionary<string, object> _cache = new Dictionary<string, object>();
        public TasksServices(TaskAPIContext context, TaskDelegates delegates, TaskQueueService queue)
        {
            _context = context;
            _delegates = delegates;
            _queue = queue;
        }


        public async Task<ActionResult<IEnumerable<Tasks<int>>>> GetAll() =>
            await _context.TaskInt.ToListAsync();

        public async Task<ActionResult<Tasks<int>>> Get(int id)
        {
            var taskId = await _context.TaskInt.FindAsync(id);

            if (taskId == null) { return new NotFoundObjectResult("No se encontro la tarea"); }

            

            return new ObjectResult(taskId);
        }

        public async Task<ActionResult> Create(Tasks<int> task)
        {
            try
            {
                
                var newTask = new Tasks<int>
                {
                    Description = task.Description,
                    DueDate = task.DueDate,
                    Status = task.Status,
                    AdditionalData = task.AdditionalData

                };


                if (!_delegates.validate(newTask))                {
                    return new BadRequestObjectResult("La fecha de entrega debe ser mayor a la actual y no pueden haber campos vacios");
                }

                await _context.TaskInt.AddAsync(newTask);
                await _context.SaveChangesAsync();

                _queue.EnqueueTask(newTask);
               

                if (task.Status =="Pending")
                {
                    return new ObjectResult(_delegates.CalculateDaysLeft(task));
                }

                return _delegates.notifyCreation(task);
                
            }
            catch (Exception ex)
            {

                return new ObjectResult($"Error {ex.Message}");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var taskFound = await _context.TaskInt.FindAsync(id);
            if(taskFound == null) { return new NotFoundObjectResult("No se encontro la tarea"); }

            _context.TaskInt.Remove(taskFound);

            await _context.SaveChangesAsync();


            return new ObjectResult("La tarea ha sido borrada exitosamente");
           
        }

        public async Task<IActionResult> Update(int id, Tasks<int> task)
        {
            var taskFound = await _context.TaskInt.FindAsync(id);
            if (taskFound == null) { return new NotFoundObjectResult("No se encontro la tarea"); }


            taskFound.Description = task.Description;
            taskFound.DueDate = task.DueDate;
            taskFound.Status = task.Status; 
            taskFound.AdditionalData = task.AdditionalData;

            await _context.SaveChangesAsync();
            return new OkObjectResult("Se edito la tarea");
        }

        public async Task<ActionResult<Tasks<int>>> GetTaskByStatusPending()
        {
            var tasksPending = await _context.TaskInt.Where(x => x.Status == "Pending").ToListAsync();

            if(tasksPending == null) { return new NotFoundObjectResult("No se encontro una tarea con ese estado"); }

            return new ObjectResult(tasksPending);
        }

        public async Task<ActionResult> CreateHighPriorityTask(Tasks<int> task)
        {
            try
            {
                var newTask = new Tasks<int>
                {
                    Description = task.Description,
                    DueDate = task.DueDate,
                    Status = "Pending",
                    AdditionalData = 1

                };

                if (!_delegates.validate(newTask))
                {
                    return new BadRequestObjectResult("La fecha de entrega debe ser mayor a la actual y no pueden haber campos vacios");
                }


                await _context.TaskInt.AddAsync(newTask);
                await _context.SaveChangesAsync();

                _queue.EnqueueTask(newTask);

                if (task.Status == "Pending")
                {
                    return new ObjectResult(_delegates.CalculateDaysLeft(task));
                }

                return _delegates.notifyCreation(task);

            }
            catch (Exception ex)
            {
                return new ObjectResult($"Error {ex.Message}");

            }
        }

        public async Task<ActionResult<IEnumerable<Tasks<int>>>> CalculateTaskCompletionRate()
        {
            //Clave para acceder al diccionario
            const string keyCache = "completion-rate";


            //Confirmar si el resultado ya esta en el diccionario
            if (_cache.TryGetValue(keyCache, out var cachedResult))
            {
                return new ObjectResult($"El porcentaje de tareas completado es: {cachedResult}");
            }

            //Utilizado para medir la velocidad en que termina de completar todo
            var stopwatch = Stopwatch.StartNew();

            var totalTasks = await _context.TaskInt.CountAsync();

            if (totalTasks == 0) { return new NotFoundObjectResult("No hay tareas registradas"); }

            var tasksCompleted = await _context.TaskInt.CountAsync(x=> x.Status =="Completed");

            double completionRate = (double)tasksCompleted / totalTasks * 100;

            _cache[keyCache] = completionRate;

            stopwatch.Stop();
            var totalElapsed = stopwatch.ElapsedMilliseconds;

            return new ObjectResult($"El porcentaje de tareas completado es: {completionRate}% Tiempo: {totalElapsed}");
        }
    }
}

