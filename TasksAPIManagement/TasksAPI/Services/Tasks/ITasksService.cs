﻿using Microsoft.AspNetCore.Mvc;
using TasksAPI.Models;

namespace TasksAPI.Services.Task
{
    public interface ITasksService<T> where T : class
    {
        Task<ActionResult<T>> GetTaskByStatusPending();
        Task<ActionResult> CreateHighPriorityTask(Tasks<int> task);
        Task<ActionResult<IEnumerable<T>>> CalculateTaskCompletionRate();

    }
}
