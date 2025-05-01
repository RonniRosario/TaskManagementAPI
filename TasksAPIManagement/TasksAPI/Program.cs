
using Microsoft.EntityFrameworkCore;
using TasksAPI.DB;
using TasksAPI.Models;
using TasksAPI.Services;

namespace TasksAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddDbContext<TaskAPIContext>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("TaskManagement"));
            });

            builder.Services.AddScoped<ICrudServices<Tasks<int>>, TasksServices>();
            builder.Services.AddScoped<ITasksService<Tasks<int>>, TasksServices>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
