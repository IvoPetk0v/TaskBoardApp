using System.Reflection;

namespace TaskBoardApp.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using Models;

    public class TaskBoardAppDbContext : IdentityDbContext
    {
        public TaskBoardAppDbContext(DbContextOptions<TaskBoardAppDbContext> options)
            : base(options)
        {
            // Database.Migrate();
        }

        public DbSet<Board> Boards { get; set; } = null!;
        public DbSet<Task> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(TaskBoardAppDbContext)));
            base.OnModelCreating(builder);
        }
    }
}