using Microsoft.EntityFrameworkCore;
using TasksAPI.Models;

namespace TasksAPI.DB
{
    public class TaskAPIContext:DbContext
    {
        public TaskAPIContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Tasks<int>> TaskInt { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }



    }
}
