using Microsoft.AspNetCore.SignalR;

namespace TasksAPI.Hubs
{
    public class TasksHub: Hub
    {
        public async Task SendTaskNotification(string message)
            => await Clients.All.SendAsync("receiveTaskNotification", message);
        
    }
}
