using Microsoft.AspNetCore.Mvc;

namespace TasksAPI.Services
{
    public interface ITasksService<T> where T : class
    {
        Task<ActionResult<T>> GetTaskByStatusPending();
    }
}
