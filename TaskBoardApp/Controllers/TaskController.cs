using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TaskBoardApp.Data;
using TaskBoardApp.Data.Models;
using TaskBoardApp.Models.Task;

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
        public async Task<IActionResult> Create(TaskFormModel model)
        {
            if (!GetBoards().Any(b => b.Id == model.BoardId))
            {
                ModelState.AddModelError(nameof(model.BoardId),"Board does not exist.");
            }

            string currentUserId = GetUserId();
            if (!ModelState.IsValid)
            {
                model.Boards = GetBoards();
                return View(model);
            }

            Data.Models.Task task = new Data.Models.Task()
            {
                Title = model.Title,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                BoardId = model.BoardId,
                OwnerId = currentUserId
            };
            await dbContext.Tasks.AddAsync(task);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("All", "Board");
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

    }

}
