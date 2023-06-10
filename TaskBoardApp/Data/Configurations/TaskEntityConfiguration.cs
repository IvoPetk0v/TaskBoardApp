using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = TaskBoardApp.Data.Models.Task;

namespace TaskBoardApp.Data.Configurations
{
    internal class TaskEntityConfiguration:IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder
                .HasOne(t => t.Board)
                .WithMany(b => b.Tasks)
                .HasForeignKey(t => t.BoardId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasData(this.GenerateTasks());
        }

        private ICollection<Task> GenerateTasks()
        {
            ICollection<Task> tasks = new HashSet<Task>()
            {
                new Task()
                {
                    Id = 1,
                    Title = "Improve CSS styles",
                    Description = "Implement better styling for all public pages",
                    CreatedOn = DateTime.UtcNow.AddDays(-200),
                    OwnerId = "4f3bfce1-35e5-4cd9-9c79-12d4c5942a7c",
                    BoardId = 1
                },
                new Task()
                {
                    Id = 2,
                    Title = "Android Client App",
                    Description = "Create Android client App for the RESTful TaskBoard service",
                    CreatedOn = DateTime.UtcNow.AddMonths(-5),
                    OwnerId = "06ea3a5d-ee0a-439f-b3c9-97c929d17639",
                    BoardId = 1
                },
                new Task()
                {
                    Id = 3,
                    Title = "Desktop Client App",
                    Description = "Create Desktop client App for the RESTful TaskBoard service",
                    CreatedOn = DateTime.UtcNow.AddMonths(-1),
                    OwnerId = "06ea3a5d-ee0a-439f-b3c9-97c929d17639",
                    BoardId = 2
                },
                new Task()
                {
                    Id = 4,
                    Title = "Create Tasks",
                    Description = "Implement [Create Task] page for adding tasks",
                    CreatedOn = DateTime.UtcNow.AddYears(-1),
                    OwnerId = "06ea3a5d-ee0a-439f-b3c9-97c929d17639",
                    BoardId = 3
                }
            };

            return tasks;
        }
    }
}