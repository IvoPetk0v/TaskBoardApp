using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoardApp.Data;
using TaskBoardApp.Data.Models;
using TaskBoardApp.Models.Task;
using Task = TaskBoardApp.Data.Models.Task;

namespace TaskBoardApp.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskBoardAppDbContext dbContext;

        public TaskController(TaskBoardAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> Create()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            TaskFormModel model = new TaskFormModel()
            {
                Boards = GetBoards()
            };
            return View(model);
        }

        private IEnumerable<TaskBoardModel> GetBoards()
            => dbContext
            .Boards
            .Select(b => new TaskBoardModel()
            {
                Id = b.Id,
                Name = b.Name
            });

        [HttpPost]
        public async Task<IActionResult> Create(TaskFormModel taskModel)
        {
            if (!GetBoards().Any(b => b.Id == taskModel.BoardId))
            {
                ModelState.AddModelError(nameof(taskModel.BoardId),"Board does not exist.");
            }

            string currentUserId = GetUserId();
            if (!ModelState.IsValid)
            {
                taskModel.Boards = GetBoards();
                return View(taskModel);
            }

            Data.Models.Task task = new Data.Models.Task()
            {
                Title = taskModel.Title,
                Description = taskModel.Description,
                CreatedOn = DateTime.Now,
                BoardId = taskModel.BoardId,
                OwnerId = currentUserId
            };
            await dbContext.Tasks.AddAsync(task);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("All", "Board");
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<IActionResult> Details(int id)
        {
            var task = await dbContext
                .Tasks
                .Where(t=>t.Id==id)
                .Select(t=> new TaskDetailsViewModel()
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    CreatedOn = t.CreatedOn.ToString("dd/MM/yyyy HH:mm"),
                    Board = t.Board.Name,
                    Owner = t.Owner.UserName
                }).FirstOrDefaultAsync();
            if (task == null)
            {
                return BadRequest();
            }

            return View(task);
        }

    }

}
