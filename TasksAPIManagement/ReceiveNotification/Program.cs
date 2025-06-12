using Microsoft.AspNetCore.SignalR.Client;


string url = "https://localhost:7112/receiveTaskNotification";

var connection = new HubConnectionBuilder()
    .WithUrl(url)
.Build();

connection.On<object>("receiveTaskNotification", (task) =>
{
    Console.WriteLine($"Tarea recibida {task}");
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Conectado");
}
catch (Exception ex)
{
    Console.WriteLine(ex);

}
Console.ReadLine();


