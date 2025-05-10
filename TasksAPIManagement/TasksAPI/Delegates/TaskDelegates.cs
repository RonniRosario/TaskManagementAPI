using Microsoft.AspNetCore.Mvc;
using TasksAPI.Models;

namespace TasksAPI.Delegates
{
    public class TaskDelegates
    {
        //validar la tarea 
        public delegate bool ValidateTask(Tasks<int> task);

        public ValidateTask validate = task =>
         !string.IsNullOrWhiteSpace(task.Description) && task.DueDate > DateTime.Now;


        //enviar una notificacion de que la tarea ha sido creada
        public Func<Tasks<int>, ActionResult> notifyCreation = task =>
                   new OkObjectResult($"Tarea Creada: {task.Description} Vencimiento: {task.DueDate}");

        //Saber los dias restantes para completar la tarea
        public Func<Tasks<int>, ActionResult> CalculateDaysLeft = task =>
         new OkObjectResult($"Tarea Creada,     Dias restantes: {(task.DueDate - DateTime.Now).Days}");
    }
}
