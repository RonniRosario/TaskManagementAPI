
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TasksAPI.DB;
using TasksAPI.Delegates;
using TasksAPI.Hubs;
using TasksAPI.JWT;
using TasksAPI.Models;
using TasksAPI.Services;
using TasksAPI.Services.AuthServices;
using TasksAPI.Services.Task;
using TasksAPI.Services.Usuarios;

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
            builder.Services.AddScoped<ICrudServices<Usuario>, UsuariosServices>();
            builder.Services.AddScoped<TaskDelegates>();
            builder.Services.AddScoped<TaskQueueService>();

            builder.Services.AddSignalR();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<RefreshTokenServices>();
            builder.Services.AddSingleton<Utilities>();
            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]!))
                };
            });

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<TasksHub>("/receiveTaskNotification");

            app.Run();
        }
    }
}
