using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasksAPI.DB;
using TasksAPI.Models;

namespace TasksAPI.Services
{
    public class TasksServices : ICrudServices<Tasks<int>>, ITasksService<Tasks<int>>
    {
        private readonly TaskAPIContext _context;
        public TasksServices(TaskAPIContext context )
        {
            _context = context;
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

                await _context.TaskInt.AddAsync(newTask);
                await _context.SaveChangesAsync();


                return new OkObjectResult("La tarea ha sido creada exitosamente");
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
            return new OkObjectResult("Se edito el usuario");
        }

        public async Task<ActionResult<Tasks<int>>> GetTaskByStatusPending()
        {
            var tasksPending = await _context.TaskInt.Where(x => x.Status == "Pending").ToListAsync();

            if(tasksPending == null) { return new NotFoundObjectResult("No se encontro una tarea con ese estado"); }

            return new ObjectResult(tasksPending);
        }
    }
}
