using Microsoft.AspNetCore.Mvc;

namespace TasksAPI.Services
{
    public interface ICrudServices<T> where T : class
    {
        Task<ActionResult<IEnumerable<T>>> GetAll();
        Task<ActionResult<T>> Get(int id);
        Task<ActionResult> Create(T item);
        Task<IActionResult> Delete(int id);
        Task<IActionResult> Update(int id, T item);
    }
}
